using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Forms;
using System.Globalization;

namespace MultipleServCharac
{
    /// <summary>
    /// service plattform:
    /// uuid="107e0004-88de-11e3-a937-0002a5d5c51b"
    /// char:
    /// sensordata uuid="107e0005-88de-11e3-a937-0002a5d5c51b"
    /// command uuid="107e0006-88de-11e3-a937-0002a5d5c51b"
    /// 
    /// service debug:
    /// uuid="107e0001-88de-11e3-a937-0002a5d5c51b"
    /// command uuid="107e0003-88de-11e3-a937-0002a5d5c51b "
    /// adc_raw 
    /// 107e0002-88de-11e3-a937-0002a5d5c51b
    /// </summary>
    public partial class Form1 : Form
    {
        private Bluegiga.BGLib bglib = new Bluegiga.BGLib();
        private Boolean isAttached = false;
        private Dictionary<string, string> portDict = new Dictionary<string, string>();



        /* ================================================================ */
        /*                BEGIN MAIN EVENT-DRIVEN APP LOGIC                 */
        /* ================================================================ */

        public const int STATE_STANDBY = 0;

        public const int STATE_SCANNING = 1;
        public const int STATE_CONNECTING = 2;

        public const int STATE_SEARCH_PLATTFORM_SERVICE = 3;
        public const int STATE_SEARCH_ATTRIBUTES = 4;
        public const int STATE_SEARCH_ATTRIBUTES_CCC = 5;
        public const int STATE_SET_ATTRIBUTES_CCC = 6;

        public const int STATE_LISTEN_MEASUREMENTS = 7;

        public const int STATE_READ_ATTR_VAL = 8;

        private int attrReadHandle = 0;


        private int app_state = STATE_STANDBY;        // current application state
        private Byte connection_handle = 0;              // connection handle (will always be 0 if only one connection happens at a time)

        private int att_handlesearch_start = 0;       // "start" handle holder during search
        private int att_handlesearch_end = 0;         // "end" handle holder during search

        private int att_handle = 0;       // heart rate measurement attribute handle
        private int att_handle_ccc = 0;   // heart rate measurement client characteristic configuration handle (to enable notifications)
        private int att_handle_readOnly = 0;

        private BLECharacteristicContainer tmpCharContainer1 = new BLECharacteristicContainer("0x02");
        private BLECharacteristicContainer tmpCharContainer2 = new BLECharacteristicContainer("0x07");

        private int att_Counter = 0;
        /// <summary>
        /// GATT ID, ATTR Handle
        /// </summary>
        private Dictionary<int, int> notKnownattrContainer = new Dictionary<int, int>();

        private int SENSOR_COUNT = 2;

        private static object o = new object();
        private BLECharacteristicContainer nextChar;

        

        /// <summary>
        /// for master/scanner devices, the "gap_scan_response" event is a common entry-like point this filters ad packets to find devices which advertise a known service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GAPScanResponseEvent(object sender, Bluegiga.BLE.Events.GAP.ScanResponseEventArgs e)
        {
            String log = String.Format("scan_response: rssi={0}, packet_type={1}, sender=[ {2}], address_type={3}, bond={4}, data=[ {5}]" + Environment.NewLine,
                (SByte)e.rssi,
                e.packet_type,
                ByteArrayToHexString(e.sender),
                e.address_type,
                e.bond,
                ByteArrayToHexString(e.data)
                );
            Console.Write(log);
            ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
           
         
            activityLabel.Image = Properties.Resources.Gnome_Network_Transmit_32;
            // pull all advertised service info from ad packet
            List<Byte[]> ad_services = new List<Byte[]>();
            Byte[] this_field = { };
            int bytes_left = 0;
            int field_offset = 0;
            for (int i = 0; i < e.data.Length; i++)
            {
                if (bytes_left == 0)
                {
                    bytes_left = e.data[i];
                    this_field = new Byte[e.data[i]];
                    field_offset = i + 1;
                }
                else
                {
                    this_field[i - field_offset] = e.data[i];
                    bytes_left--;
                    if (bytes_left == 0)
                    {
                        if (this_field[0] == 0x02 || this_field[0] == 0x03)
                        {
                            // partial or complete list of 16-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(2).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x04 || this_field[0] == 0x05)
                        {
                            // partial or complete list of 32-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(4).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x06 || this_field[0] == 0x07)
                        {
                            // partial or complete list of 128-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(16).ToArray());
                        }
                    }
                }
            }

            // check for service UUID plattform
            if (ad_services.Any(a => a.SequenceEqual(new Byte[] { 0x1b, 0xc5, 0xd5, 0xa5, 0x02, 0x00, 0x37, 0xa9, 0xe3, 0x11, 0xde, 0x88, 0x04, 0x00, 0x7e, 0x10 })))
            {
                // connect to this device
                Byte[] cmd = bglib.BLECommandGAPConnectDirect(e.sender, e.address_type, 0x20, 0x30, 0x100, 0); // 125ms interval, 125ms window, active scanning
                // DEBUG: display bytes written
                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> Length  ({0})  Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                bglib.SendCommand(serialAPI, cmd);
                //while (bglib.IsBusy()) ;

                // update state
                app_state = STATE_CONNECTING;
            }
        }

        /// <summary>
        /// New connection is established
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">StatusEventArgs</param>
        public void ConnectionStatusEvent(object sender, Bluegiga.BLE.Events.Connection.StatusEventArgs e)
        {
            String log = String.Format("ble_evt_connection_status: connection={0}, flags={1}, address=[ {2}], address_type={3}, conn_interval={4}, timeout={5}, latency={6}, bonding={7}" + Environment.NewLine,
                e.connection,
                e.flags,
                ByteArrayToHexString(e.address),
                e.address_type,
                e.conn_interval,
                e.timeout,
                e.latency,
                e.bonding
                );
            Console.Write(log);
            ThreadSafeDelegate(delegate { txtLog.AppendText(log); });

            if ((e.flags & 0x05) == 0x05)
            {
                // connected, now perform service discovery
                connection_handle = e.connection;
                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Connected to {0}", ByteArrayToHexString(e.address)) + Environment.NewLine); });
                Byte[] cmd = bglib.BLECommandATTClientReadByGroupType(e.connection, 0x0001, 0xFFFF, new Byte[] { 0x00, 0x28 }); // "service" UUID is 0x2800 (little-endian for UUID uint8array)
               
                // DEBUG: display bytes written
                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> Length: ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                bglib.SendCommand(serialAPI, cmd);
                //while (bglib.IsBusy()) ;

                // update state
                app_state = STATE_SEARCH_PLATTFORM_SERVICE;

            }
        }

        /// <summary>
        /// Information (uuid) about serv. found, have to determine we know it (Serv. List). 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GroupFoundEventArgs</param>
        public void ATTClientGroupFoundEvent(object sender, Bluegiga.BLE.Events.ATTClient.GroupFoundEventArgs e)
        {

            String log = String.Format("group_found: connection={0}, start={1}, end={2}, uuid=[ {3}]" + Environment.NewLine,
                e.connection,
                e.start,
                e.end,
                ByteArrayToHexString(e.uuid)
                );
            Console.Write(log);

            ThreadSafeDelegate(delegate { txtLog.AppendText(log); });

            // found "service" attribute groups check for sensorplattform and debug service
            if (e.uuid.SequenceEqual(new Byte[] { 0x1b, 0xc5, 0xd5, 0xa5, 0x02, 0x00, 0x37, 0xa9, 0xe3, 0x11, 0xde, 0x88, 0x04, 0x00, 0x7e, 0x10 }))
            //  || e.uuid.SequenceEqual(new Byte[] { 0x1b, 0xc5, 0xd5, 0xa5, 0x02, 0x00, 0x37, 0xa9, 0xe3, 0x11, 0xde, 0x88, 0x01, 0x01, 0x7e, 0x10 }))
            {
                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("attribute group found service {0} start={1}, end={2}", e.uuid, e.start, e.end) + Environment.NewLine); });
                att_handlesearch_start = e.start;
                att_handlesearch_end = e.end;
               // app_state = STATE_SEARCH_PLATTFORM_SERVICE;
            }
            else
            {
                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("!!! NO group found") + Environment.NewLine); });
            }
        }

        /// <summary>
        /// Information (uuid) about char. found, have to determine which type. The CCC or standard char. uuid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">FindInformationFoundEventArgs</param>
        public void ATTClientFindInformationFoundEvent(object sender, Bluegiga.BLE.Events.ATTClient.FindInformationFoundEventArgs e)
        {
            String log = String.Format("information_found: connection={0}, chrhandle={1}, uuid=[ {2}]" + Environment.NewLine,
                e.connection,
                e.chrhandle,
                ByteArrayToHexString(e.uuid)
                );
            Console.Write(log);
            ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (app_state == STATE_SEARCH_ATTRIBUTES)
            {
                notKnownattrContainer.Add( (int) e.uuid[e.uuid.Count() - 4], e.chrhandle);
                // check for characteristics
                if (e.uuid.SequenceEqual(new Byte[] { 0x1b, 0xc5, 0xd5, 0xa5, 0x02, 0x00, 0x37, 0xa9, 0xe3, 0x11, 0xde, 0x88, 0x02, 0x00, 0x7e, 0x10 }))
                {

                    ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute {0} set handle: handle={0}", e.chrhandle) + Environment.NewLine); });
                    tmpCharContainer1.Handle = e.chrhandle;

                    //    att_handle_readOnly = 1;
                    //  att_List[att_Counter] = tmpCharContainer;
                    att_Counter++;
                    app_state = STATE_SEARCH_ATTRIBUTES_CCC;

                }
                else if (e.uuid.SequenceEqual(new Byte[] { 0x1b, 0xc5, 0xd5, 0xa5, 0x02, 0x00, 0x37, 0xa9, 0xe3, 0x11, 0xde, 0x88, 0x07, 0x00, 0x7e, 0x10 }))
                {
                    ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute {0} set handle: handle={0}", e.chrhandle) + Environment.NewLine); });
                    tmpCharContainer2.Handle = e.chrhandle;
                    //    att_handle_readOnly = 1;
                    //  att_List[att_Counter] = tmpCharContainer;
                    att_Counter++;
                    app_state = STATE_SEARCH_ATTRIBUTES_CCC;
                }
            }
            // check for subsequent client characteristic configuration (UUID=0x2902)
            else
            {
                if (app_state == STATE_SEARCH_ATTRIBUTES_CCC)
                {
                    if (e.uuid.SequenceEqual(new Byte[] { 0x02, 0x29 }))
                    {
                        app_state = STATE_SEARCH_ATTRIBUTES;

                        ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute 0x2902 set ccc: handle={0}", e.chrhandle) + Environment.NewLine); });
                      
                        if (!tmpCharContainer1.isCCCSet())
                        {
                            tmpCharContainer1.Handle_ccc = e.chrhandle;
                        } 
                        else 
                        //if (tmpCharContainer1.isCCCSet() && !tmpCharContainer2.isCCCSet())
                        {
                                tmpCharContainer2.Handle_ccc = e.chrhandle;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  The last started procedure has completed. E.g. we ended searching attr. or serv.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">ProcedureCompletedEventArgs</param>
        public void ATTClientProcedureCompletedEvent(object sender, Bluegiga.BLE.Events.ATTClient.ProcedureCompletedEventArgs e)
        {
            lock (o)
            {
                String log = String.Format("procedure_completed: connection={0}, result={1}, chrhandle={2}" + Environment.NewLine,
                    e.connection,
                    e.result,
                    e.chrhandle
                    );
                Console.Write(log);
                ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
                Byte[] cmd;
                // check if we just finished searching for attr in services
                switch (app_state)
                {
                    case  STATE_SEARCH_PLATTFORM_SERVICE:
                        // set to value when new service found
                        if (att_handlesearch_end > 0)
                        {
                            // found a service, so now search for the attributes inside
                            cmd = bglib.BLECommandATTClientFindInformation(e.connection, (UInt16)att_handlesearch_start, (UInt16)att_handlesearch_end);
                          //  Byte[] cmd = bglib.BLECommandATTClientReadByType(e.connection, (ushort)att_handlesearch_start, (ushort)att_handlesearch_end, new byte[] { 0x03, 0x28 }); 
                        
                            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Sending find information command \n =>   Length: ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                            bglib.SendCommand(serialAPI, cmd);
                            //while (bglib.IsBusy()) ;

                            // update state
                            app_state = STATE_SEARCH_ATTRIBUTES;
                            att_handlesearch_start = 0;
                            att_handlesearch_end = 0;
                        }
                        else
                        {
                            ThreadSafeDelegate(delegate { txtLog.AppendText("Could not find services" + Environment.NewLine); });
                        }
                    break;
                    // characteristics search completed -- send notification command to first char
                    case STATE_SEARCH_ATTRIBUTES: // lasts ATTRccc val set in model - send notifications
              
                        ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Set STATE to SET_CCC \n ==> SET CCC Command Handle {0} " , tmpCharContainer1.Handle_ccc) + Environment.NewLine); });
                        
                        app_state = STATE_SET_ATTRIBUTES_CCC;

                        cmd = bglib.BLECommandATTClientAttributeWrite(e.connection, (UInt16)tmpCharContainer1.Handle_ccc, new Byte[] { 0x01 });

                        bglib.SendCommand(serialAPI, cmd);
                    break;
                    case STATE_SET_ATTRIBUTES_CCC:

                        cmd = bglib.BLECommandATTClientAttributeWrite(e.connection, (UInt16)tmpCharContainer2.Handle_ccc, new Byte[] { 0x01 });
                        ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("STATE_SETATTR -- send last notify !!!!! to {0}", tmpCharContainer2.Handle_ccc) + Environment.NewLine); });
               
                        // we done notify for all characs 
                        app_state = STATE_LISTEN_MEASUREMENTS;

                        readButton.Enabled = true; 
                        bglib.SendCommand(serialAPI, cmd);
                     /*   // set the state to measure when all sensors connected
                        if (att_Counter == SENSOR_COUNT)
                        {
                            app_state = STATE_LISTENING_MEASUREMENTS;
                        } */
                    break;
                    case STATE_READ_ATTR_VAL:

                        ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Setting appstate back to READ Measure" + Environment.NewLine)); });
                   
                    break;
                    default : ThreadSafeDelegate(delegate { txtLog.AppendText("!!! STATE NOT FOUND" + Environment.NewLine); });
                    break;
                }
            }
        }

        /// <summary>
        /// A new value arrived via notification or read request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">AttributeValueEventArgs</param>
        public void ATTClientAttributeValueEvent(object sender, Bluegiga.BLE.Events.ATTClient.AttributeValueEventArgs e)
        {
            lock (o)
            {
                String log = String.Format("attribute_value: connection={0}, atthandle={1}, type={2}, value=[ {3}]" + Environment.NewLine,
                    e.connection,
                    e.atthandle,
                    e.type,
                    ByteArrayToHexString(e.value)
                    );
                Console.Write(log);
                //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
                int measurement = 0;
              
                // each connection has its own list of serv. and char.
                switch (e.connection)
                {
                    case 0:
                        // con - get serv. , get char list ... alle !!
                        // Only display char. we know so check the List
                      //  for (int i = 0; i < att_List.Length; i++)
                       // {
                            if (e.atthandle == tmpCharContainer1.Handle)
                            {
                                measurement = ByteArrayToInt(e.value);
                                tmpCharContainer1.Value = measurement;
                                // display actual measurement
                              
                                Console.WriteLine(String.Format("Conn.: {0}   Sensor Data: {1} ListNr.: {2} ", 0, measurement, 0) + Environment.NewLine);
                              
                                    ThreadSafeDelegate(delegate { sensor_distance_value_label.Text = "" + measurement + " cm"; });
                                     
                            }
                            if (e.atthandle == tmpCharContainer2.Handle)
                            {
                                measurement = ByteArrayToInt(e.value);
                                tmpCharContainer1.Value = measurement;
                                // display actual measurement
                              
                                Console.WriteLine(String.Format("Conn.: {0}   Sensor Data: {1} ListNr.: {2} ", 0, measurement, 1) + Environment.NewLine);
                              
                                ThreadSafeDelegate(delegate { sensor_distance_raw_label.Text = "" + measurement; });
                                
                            }                            
                            if (app_state == STATE_READ_ATTR_VAL) 
                            {
                                ThreadSafeDelegate(delegate { txtLog.AppendText("Read val: \n" + e.value); });
                            }
                        break;
                    // Next connection...
          /*          case 1:
                        // Only display char. we know so check the List
                        for (int i = 0; i < att_List.Length; i++)
                        {
                            if (att_List[i] != null && e.atthandle == att_List[i].Handle)
                            {
                                measurement = ByteArrayToInt(e.value);
                                att_List[i].Value = measurement;
                                // display actual measurement
                                ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Conn.: {0}   Sensor Data: {1} ListNr.: {2} ", 1, measurement, i) + Environment.NewLine); });
                            }
                        }
                        break;*/

                    default: ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("!!! No suitable connection {0} for Sensor Data: {1}  with handle {2}", e.connection, e.value, e.atthandle) + Environment.NewLine); });
                        break;
                }
            }
        }

        /* ================================================================ */
        /*                 END MAIN EVENT-DRIVEN APP LOGIC                  */
        /* ================================================================ */



        /// <summary>
        ///  Thread-safe operations from event handlers
        /// </summary>
        /// <param name="method"></param>
        public void ThreadSafeDelegate(MethodInvoker method)
        {
          
                if (InvokeRequired)
                    BeginInvoke(method);
                else
                    method.Invoke();
            
        }

        /// <summary>
        /// Convert byte array to "00 11 22 33 44 55 " string
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public string ByteArrayToHexString(Byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        /// <summary>
        /// Convert byte array to int
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public int ByteArrayToInt(Byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            string s = hex.ToString();
            int tmp = 0;
            tmp = Int32.Parse(s, NumberStyles.HexNumber);
            return tmp;
        }

        /// <summary>
        /// Serial port event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(
                                object sender,
                                System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            Byte[] inData = new Byte[sp.BytesToRead];

            // read all available bytes from serial port in one chunk
            sp.Read(inData, 0, sp.BytesToRead);

            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("<=  Length: ({0}) Data: [ {1}]", inData.Length, ByteArrayToHexString(inData)) + Environment.NewLine); });

            // parse all bytes read through BGLib parser
            for (int i = 0; i < inData.Length; i++)
            {
                bglib.Parse(inData[i]);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // initialize list of ports
            btnRefresh_Click(sender, e);
            try
            {
                // initialize COM port combobox with list of ports
                comboPorts.DataSource = new BindingSource(portDict, null);
                comboPorts.DisplayMember = "Value";
                comboPorts.ValueMember = "Key";
            }
            catch (System.ArgumentException)
            {
                //
            }

            // initialize serial port with all of the normal values (should work with BLED112 on USB)
            serialAPI.Handshake = System.IO.Ports.Handshake.RequestToSend;
            serialAPI.BaudRate = 256000;
            serialAPI.DataBits = 8;
            serialAPI.StopBits = System.IO.Ports.StopBits.One;
            serialAPI.Parity = System.IO.Ports.Parity.None;
            serialAPI.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceivedHandler);

            // initialize BGLib events we'll need for this script
            bglib.BLEEventGAPScanResponse += new Bluegiga.BLE.Events.GAP.ScanResponseEventHandler(this.GAPScanResponseEvent);
            bglib.BLEEventConnectionStatus += new Bluegiga.BLE.Events.Connection.StatusEventHandler(this.ConnectionStatusEvent);
            bglib.BLEEventATTClientGroupFound += new Bluegiga.BLE.Events.ATTClient.GroupFoundEventHandler(this.ATTClientGroupFoundEvent);
            bglib.BLEEventATTClientFindInformationFound += new Bluegiga.BLE.Events.ATTClient.FindInformationFoundEventHandler(this.ATTClientFindInformationFoundEvent);
            bglib.BLEEventATTClientProcedureCompleted += new Bluegiga.BLE.Events.ATTClient.ProcedureCompletedEventHandler(this.ATTClientProcedureCompletedEvent);
            bglib.BLEEventATTClientAttributeValue += new Bluegiga.BLE.Events.ATTClient.AttributeValueEventHandler(this.ATTClientAttributeValueEvent);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // get a list of all available ports on the system
            portDict.Clear();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SerialPort");
                //string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (String.Format("{0}", queryObj["Caption"]).Contains("Bluetooth"))
                    {
                        portDict.Add(String.Format("{0}", queryObj["DeviceID"]), String.Format("{0} - {1}", queryObj["DeviceID"], queryObj["Caption"]));

                    }
                }
            }
            catch (ManagementException ex)
            {
                portDict.Add("0", "Error " + ex.Message);
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isAttached)
                {
                    txtLog.AppendText("Attach to serial '" + comboPorts.SelectedValue.ToString() + "'..." + Environment.NewLine);
                    serialAPI.PortName = comboPorts.SelectedValue.ToString();
                    serialAPI.Open();
                    txtLog.AppendText("Attached" + Environment.NewLine);
                    isAttached = true;
                    btnAttach.BackColor = Color.LightGreen;
                    btnGo.Enabled = true;
                    btnReset.Enabled = true;
                }
                else
                {
                    txtLog.AppendText("Detach from serial..." + Environment.NewLine);
                    serialAPI.Close();
                    txtLog.AppendText("Port closed" + Environment.NewLine);
                    isAttached = false;
                    btnAttach.Text = "Attach";
                    btnGo.Enabled = false;
                    btnReset.Enabled = false;
                }
            }
            catch (System.ArgumentException ex)
            {
                txtLog.AppendText(ex.Message + Environment.NewLine);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            // start the scan/connect process now
            Byte[] cmd;

            // set scan parameters
            cmd = bglib.BLECommandGAPSetScanParameters(0xC8, 0xC8, 1); // 125ms interval, 125ms window, active scanning
            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>  Length: ({0})  Data:  [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // begin scanning for BLE peripherals
            cmd = bglib.BLECommandGAPDiscover(1); // generic discovery mode
            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>  Length:  ({0})  Data:  [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // update state
            app_state = STATE_SCANNING;

            // disable "GO" button since we already started, and sending the same commands again sill not work right
            btnGo.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // stop everything we're doing, if possible
            Byte[] cmd;

            // disconnect if connected
            cmd = bglib.BLECommandConnectionDisconnect(0);
            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>   Length:  ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // stop scanning if scanning
            cmd = bglib.BLECommandGAPEndProcedure();
            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>   Lenght:  ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // stop advertising if advertising
            cmd = bglib.BLECommandGAPSetMode(0, 0);
            // DEBUG: display bytes read
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>   Lenght   ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // enable "GO" button to allow them to start again
            btnGo.Enabled = true;

            // update state
            app_state = STATE_STANDBY;
        }

        private void Characteristic1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void comboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void sensorLabel1_Click(object sender, EventArgs e)
        {

        }

        private void PlattformService_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void readButton_Click(object sender, EventArgs e)
        {
            Byte[] cmd;
            app_state = STATE_READ_ATTR_VAL;
            // send cmd
            cmd = bglib.BLECommandATTClientReadByHandle(0, (ushort) attrReadHandle);
            // display bytes send
            ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=>   Length:  ({0}) Data:   [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            bglib.SendCommand(serialAPI, cmd);
        }

    }
}