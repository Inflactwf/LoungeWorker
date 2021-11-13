using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Windows.Forms.Timer;

namespace CreateTrade
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _csgo_AutoBumpTimer;
        private Timer _dota_AutoBumpTimer;
        private const string KnifeSymbol = "$([char]9733)";
        private const string StatTrakSymbol = "$([char]8482)";
        private AdvancedInformation Config;
        private AdvHelper Blocklist;
        private BackgroundWorker D2Worker;
        private BackgroundWorker CSGLWorker;
        private BackgroundWorker BlockWorker;
        private BackgroundWorker RepeatBlockWorker;
        private FileInfo BlockListInfo;
        private Process DotaProcess;
        private Process CSGOProcess;
        private bool _dota_AutoRestart;
        private bool _csgo_AutoRestart;

        private const string csgoloungesite = "https://old.csgolounge.com";
        private const string dota2loungesite = "https://old.dota2lounge.com";
        private const string settingspath = @"config/settings.cfg";
        private const string dota2postpath = @"config/Dota2Post.txt";
        private const string dota2trade1path = @"config/Dota2Trade1.txt";
        private const string dota2trade2path = @"config/Dota2Trade2.txt";
        private const string dota2trade3path = @"config/Dota2Trade3.txt";
        private const string dota2trade4path = @"config/Dota2Trade4.txt";
        private const string dota2trade5path = @"config/Dota2Trade5.txt";
        private const string dota2trade6path = @"config/Dota2Trade6.txt";
        private const string csgotrade1path = @"config/CSGOTrade1.txt";
        private const string csgotrade2path = @"config/CSGOTrade2.txt";
        private const string csgotrade3path = @"config/CSGOTrade3.txt";
        private const string csgotrade4path = @"config/CSGOTrade4.txt";
        private const string csgotrade5path = @"config/CSGOTrade5.txt";
        private const string csgotrade6path = @"config/CSGOTrade6.txt";
        private string blocklistpath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\config\Blocklist.txt";

        public MainWindow()
        {
            InitializeComponent();
            Config = new AdvancedInformation(settingspath);
            Blocklist = new AdvHelper();
            BlockListInfo = new FileInfo(blocklistpath);

            D2Worker = new BackgroundWorker();
            D2Worker.WorkerSupportsCancellation = true;
            D2Worker.RunWorkerCompleted += D2Worker_RunWorkerCompleted;

            CSGLWorker = new BackgroundWorker();
            CSGLWorker.WorkerSupportsCancellation = true;
            CSGLWorker.RunWorkerCompleted += CSGLWorker_RunWorkerCompleted;

            BlockWorker = new BackgroundWorker();
            BlockWorker.WorkerSupportsCancellation = true;
            BlockWorker.WorkerReportsProgress = true;
            BlockWorker.DoWork += BlockWorker_DoWork;
            BlockWorker.ProgressChanged += BlockWorker_ProgressChanged;
            BlockWorker.RunWorkerCompleted += BlockWorker_RunWorkerCompleted;

            RepeatBlockWorker = new BackgroundWorker();
            RepeatBlockWorker.WorkerSupportsCancellation = true;
            RepeatBlockWorker.WorkerReportsProgress = true;
            RepeatBlockWorker.DoWork += RepeatBlockWorker_DoWork;
            RepeatBlockWorker.ProgressChanged += RepeatBlockWorker_ProgressChanged;
            RepeatBlockWorker.RunWorkerCompleted += RepeatBlockWorker_RunWorkerCompleted;
        }

        private void _csgo_RunButton_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box.Text;
            string item2 = _csgo_Item2Box.Text;
            string item3 = _csgo_Item3Box.Text;
            string item4 = _csgo_Item4Box.Text;
            string item5 = _csgo_Item5Box.Text;
            string item6 = _csgo_Item6Box.Text;
            string item7 = _csgo_Item7Box.Text;
            string item8 = _csgo_Item8Box.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _csgo_RunButton1_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box1.Text;
            string item2 = _csgo_Item2Box1.Text;
            string item3 = _csgo_Item3Box1.Text;
            string item4 = _csgo_Item4Box1.Text;
            string item5 = _csgo_Item5Box1.Text;
            string item6 = _csgo_Item6Box1.Text;
            string item7 = _csgo_Item7Box1.Text;
            string item8 = _csgo_Item8Box1.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox1.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox1.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _csgo_RunButton2_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box2.Text;
            string item2 = _csgo_Item2Box2.Text;
            string item3 = _csgo_Item3Box2.Text;
            string item4 = _csgo_Item4Box2.Text;
            string item5 = _csgo_Item5Box2.Text;
            string item6 = _csgo_Item6Box2.Text;
            string item7 = _csgo_Item7Box2.Text;
            string item8 = _csgo_Item8Box2.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox2.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox2.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _csgo_RunButton3_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box3.Text;
            string item2 = _csgo_Item2Box3.Text;
            string item3 = _csgo_Item3Box3.Text;
            string item4 = _csgo_Item4Box3.Text;
            string item5 = _csgo_Item5Box3.Text;
            string item6 = _csgo_Item6Box3.Text;
            string item7 = _csgo_Item7Box3.Text;
            string item8 = _csgo_Item8Box3.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox3.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox3.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _csgo_RunButton4_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box4.Text;
            string item2 = _csgo_Item2Box4.Text;
            string item3 = _csgo_Item3Box4.Text;
            string item4 = _csgo_Item4Box4.Text;
            string item5 = _csgo_Item5Box4.Text;
            string item6 = _csgo_Item6Box4.Text;
            string item7 = _csgo_Item7Box4.Text;
            string item8 = _csgo_Item8Box4.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox4.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox4.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _csgo_RunButton5_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _csgo_Item1Box5.Text;
            string item2 = _csgo_Item2Box5.Text;
            string item3 = _csgo_Item3Box5.Text;
            string item4 = _csgo_Item4Box5.Text;
            string item5 = _csgo_Item5Box5.Text;
            string item6 = _csgo_Item6Box5.Text;
            string item7 = _csgo_Item7Box5.Text;
            string item8 = _csgo_Item8Box5.Text;

            if (item1.Length != 0)
            {
                if (item1.Contains("★"))
                {
                    item1 = item1.Replace("★", KnifeSymbol);
                }
                if (item1.Contains("™"))
                {
                    item1 = item1.Replace("™", StatTrakSymbol);
                }

                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item2.Contains("★"))
                {
                    item2 = item2.Replace("★", KnifeSymbol);
                }
                if (item2.Contains("™"))
                {
                    item2 = item2.Replace("™", StatTrakSymbol);
                }

                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item3.Contains("★"))
                {
                    item3 = item3.Replace("★", KnifeSymbol);
                }
                if (item3.Contains("™"))
                {
                    item3 = item3.Replace("™", StatTrakSymbol);
                }

                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item4.Contains("★"))
                {
                    item4 = item4.Replace("★", KnifeSymbol);
                }
                if (item4.Contains("™"))
                {
                    item4 = item4.Replace("™", StatTrakSymbol);
                }

                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item5.Contains("★"))
                {
                    item5 = item5.Replace("★", KnifeSymbol);
                }
                if (item5.Contains("™"))
                {
                    item5 = item5.Replace("™", StatTrakSymbol);
                }

                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item6.Contains("★"))
                {
                    item6 = item6.Replace("★", KnifeSymbol);
                }
                if (item6.Contains("™"))
                {
                    item6 = item6.Replace("™", StatTrakSymbol);
                }

                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item7.Contains("★"))
                {
                    item7 = item7.Replace("★", KnifeSymbol);
                }
                if (item7.Contains("™"))
                {
                    item7 = item7.Replace("™", StatTrakSymbol);
                }

                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                if (item8.Contains("★"))
                {
                    item8 = item8.Replace("★", KnifeSymbol);
                }
                if (item8.Contains("™"))
                {
                    item8 = item8.Replace("™", StatTrakSymbol);
                }

                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + csgoloungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\";" +
                " \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\";" +
                " \"recaptcha\"=\"" + _csgo_CaptchaBox5.Text + "\";" +
                " \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"application/json\"; \"referer\"=\"" + csgoloungesite + "/addtrade\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\" -Body ([System.Text.Encoding]::UTF8.GetBytes(\"{`\"sell_items`\":" +
                "{`\"steam_ids`\":[`\"" + items + "\"],`\"virtual_ids`\":[]},`\"buy_items`\":" +
                "{`\"steam_ids`\":[],`\"virtual_ids`\":[`\"Any Offers`\",`\"Any Knife`\"]},`" +
                "\"description`\":`\"" + _csgo_DescriptionBox5.Text + "`\"}\"))");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box.Text;
            string item2 = _dota_CreateTrade_Item2Box.Text;
            string item3 = _dota_CreateTrade_Item3Box.Text;
            string item4 = _dota_CreateTrade_Item4Box.Text;
            string item5 = _dota_CreateTrade_Item5Box.Text;
            string item6 = _dota_CreateTrade_Item6Box.Text;
            string item7 = _dota_CreateTrade_Item7Box.Text;
            string item8 = _dota_CreateTrade_Item8Box.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton1_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box1.Text;
            string item2 = _dota_CreateTrade_Item2Box1.Text;
            string item3 = _dota_CreateTrade_Item3Box1.Text;
            string item4 = _dota_CreateTrade_Item4Box1.Text;
            string item5 = _dota_CreateTrade_Item5Box1.Text;
            string item6 = _dota_CreateTrade_Item6Box1.Text;
            string item7 = _dota_CreateTrade_Item7Box1.Text;
            string item8 = _dota_CreateTrade_Item8Box1.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox1.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox1.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton2_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box2.Text;
            string item2 = _dota_CreateTrade_Item2Box2.Text;
            string item3 = _dota_CreateTrade_Item3Box2.Text;
            string item4 = _dota_CreateTrade_Item4Box2.Text;
            string item5 = _dota_CreateTrade_Item5Box2.Text;
            string item6 = _dota_CreateTrade_Item6Box2.Text;
            string item7 = _dota_CreateTrade_Item7Box2.Text;
            string item8 = _dota_CreateTrade_Item8Box2.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox2.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox2.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton3_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box3.Text;
            string item2 = _dota_CreateTrade_Item2Box3.Text;
            string item3 = _dota_CreateTrade_Item3Box3.Text;
            string item4 = _dota_CreateTrade_Item4Box3.Text;
            string item5 = _dota_CreateTrade_Item5Box3.Text;
            string item6 = _dota_CreateTrade_Item6Box3.Text;
            string item7 = _dota_CreateTrade_Item7Box3.Text;
            string item8 = _dota_CreateTrade_Item8Box3.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox3.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox3.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton4_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box4.Text;
            string item2 = _dota_CreateTrade_Item2Box4.Text;
            string item3 = _dota_CreateTrade_Item3Box4.Text;
            string item4 = _dota_CreateTrade_Item4Box4.Text;
            string item5 = _dota_CreateTrade_Item5Box4.Text;
            string item6 = _dota_CreateTrade_Item6Box4.Text;
            string item7 = _dota_CreateTrade_Item7Box4.Text;
            string item8 = _dota_CreateTrade_Item8Box4.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox4.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox4.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_CreateTrade_RunButton5_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_CreateTrade_Item1Box5.Text;
            string item2 = _dota_CreateTrade_Item2Box5.Text;
            string item3 = _dota_CreateTrade_Item3Box5.Text;
            string item4 = _dota_CreateTrade_Item4Box5.Text;
            string item5 = _dota_CreateTrade_Item5Box5.Text;
            string item6 = _dota_CreateTrade_Item6Box5.Text;
            string item7 = _dota_CreateTrade_Item7Box5.Text;
            string item8 = _dota_CreateTrade_Item8Box5.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/\"; \"origin\"=\"" + dota2loungesite + "\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"accept\"=\"application/json\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"sec-fetch-dest\"=\"empty\"; " +
                    "\"recaptcha\"=\"" + _dota_CaptchaBox5.Text + "\"; \"dnt\"=\"1\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + dota2loungesite + "/addtrade\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`\"sell_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]},`\"buy_items`\":{`" +
                    "\"steam_ids`\":[],`" +
                    "\"virtual_ids`\":[`\"Offers`\",`\"Any Mythical`\",`\"Any Legendary`\",`\"Any Ancient`\",`\"Any Immortal`\",`\"+ More`\"]},`" +
                    "\"description`\":`\"" + _dota_CreateTrade_DescriptionBox5.Text + "`\"}\"");

                MessageBox.Show("Сделка успешно создана.", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_PostMessage_PostMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            string items = "";
            string item1 = _dota_PostMessage_Item1Box.Text;
            string item2 = _dota_PostMessage_Item2Box.Text;
            string item3 = _dota_PostMessage_Item3Box.Text;
            string item4 = _dota_PostMessage_Item4Box.Text;
            string item5 = _dota_PostMessage_Item5Box.Text;
            string item6 = _dota_PostMessage_Item6Box.Text;
            string item7 = _dota_PostMessage_Item7Box.Text;
            string item8 = _dota_PostMessage_Item8Box.Text;

            if (item1.Length != 0)
            {
                if (item2.Length != 0)
                {
                    item1 = item1 + "`\",`\"";
                }
                else
                {
                    item1 = item1 + "`";
                }

                items += item1;
            }

            if (item2.Length != 0)
            {
                if (item3.Length != 0)
                {
                    item2 = item2 + "`\",`\"";
                }
                else
                {
                    item2 = item2 + "`";
                }

                items += item2;
            }

            if (item3.Length != 0)
            {
                if (item4.Length != 0)
                {
                    item3 = item3 + "`\",`\"";
                }
                else
                {
                    item3 = item3 + "`";
                }

                items += item3;
            }

            if (item4.Length != 0)
            {
                if (item5.Length != 0)
                {
                    item4 = item4 + "`\",`\"";
                }
                else
                {
                    item4 = item4 + "`";
                }

                items += item4;
            }

            if (item5.Length != 0)
            {
                if (item6.Length != 0)
                {
                    item5 = item5 + "`\",`\"";
                }
                else
                {
                    item5 = item5 + "`";
                }

                items += item5;
            }

            if (item6.Length != 0)
            {
                if (item7.Length != 0)
                {
                    item6 = item6 + "`\",`\"";
                }
                else
                {
                    item6 = item6 + "`";
                }

                items += item6;
            }

            if (item7.Length != 0)
            {
                if (item8.Length != 0)
                {
                    item7 = item7 + "`\",`\"";
                }
                else
                {
                    item7 = item7 + "`";
                }

                items += item7;
            }

            if (item8.Length != 0)
            {
                item8 = item8 + "`";
                items += item8;
            }

            try
            {
                RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_PostMessage_TradeIDBox.Text + "/replies/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; " +
                    "\"path\"=\"/v1/trades/250828256/replies/\"; " +
                    "\"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"origin\"=\"" + dota2loungesite + "\"; \"sec-fetch-dest\"=\"empty\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"dnt\"=\"1\"; \"accept\"=\"*/*\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; " +
                    "\"referer\"=\"" + dota2loungesite + "/trades/" + _dota_PostMessage_TradeIDBox.Text + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; " +
                    "\"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"} -ContentType \"application/json\" -Body \"{`" +
                    "\"message`\":`\"<p>" + _dota_PostMessage_TextToPostBox.Text + "</p>`\"" +
                    ",`\"bid_items`\":{`" +
                    "\"steam_ids`\":[`\"" + items + "\"],`" +
                    "\"virtual_ids`\":[]}}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void _dota_AutoBumpStartBtn_Click(object sender, RoutedEventArgs e)
        {
            _dota_AutoBumpStartBtn.IsEnabled = false;
            _dota_AutoBumpStopBtn.IsEnabled = true;
            _dota_AutoBumpTimer.Start();
        }

        private void _csgo_AutoBumpStartBtn_Click(object sender, RoutedEventArgs e)
        {
            _csgo_AutoBumpStartBtn.IsEnabled = false;
            _csgo_AutoBumpStopBtn.IsEnabled = true;
            _csgo_AutoBumpTimer.Start();
        }

        private void _csgo_AutoBumpTimer_Tick(object source, EventArgs e)
        {
            CSGLWorker.RunWorkerAsync();
        }

        private void _dota_AutoBumpTimer_Tick(object source, EventArgs e)
        {
            D2Worker.RunWorkerAsync();
        }

        private void _csgo_AutoBumpStopBtn_Click(object sender, RoutedEventArgs e)
        {
            _csgo_AutoBumpStopBtn.IsEnabled = false;
            _csgo_AutoBumpStartBtn.IsEnabled = true;
            _csgo_AutoBumpTimer.Enabled = false;
            _csgo_AutoBumpTimer.Interval = 1;
        }

        private void _dota_AutoBumpStopBtn_Click(object sender, RoutedEventArgs e)
        {
            _dota_AutoBumpStopBtn.IsEnabled = false;
            _dota_AutoBumpStartBtn.IsEnabled = true;
            _dota_AutoBumpTimer.Enabled = false;
            _dota_AutoBumpTimer.Interval = 1;
        }

        private void _general_RepeatBlockButton_Click(object sender, RoutedEventArgs e)
        {
            BlockListInfo.Refresh();
            if (BlockListInfo.Exists)
            {
                if (BlockListInfo.Length != 0)
                {
                    _general_RepeatBlockButton.IsEnabled = false;
                    _general_BlockButton.IsEnabled = false;

                    BlockingProgressBar.Value = 0;
                    BlockingProgressBar.Maximum = Blocklist.ReadSections(blocklistpath).Length;
                    RepeatBlockWorker.RunWorkerAsync();
                }
            }
        }

        private void RepeatBlockWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BlockingProgressBar.Value = e.ProgressPercentage;
        }

        private void RepeatBlockWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
            {
                countblocked = 0;
                countalreadyblocked = 0;
                countunknownerror = 0;
                int allcounts = 0;

                foreach (var profile in Blocklist.ReadSections(blocklistpath))
                {
                    if (profile.Length != 0)
                    {
                        try
                        {
                            RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/bans/" + ProfileShortID(profile) + "/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; " +
            "\"path\"=\"/v1/bans/" + ProfileShortID(profile) + "/\"; " +
            "\"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\"; \"origin\"=\"" + csgoloungesite + "\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36\"; \"accept\"=\"*/*\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; " +
            "\"referer\"=\"" + csgoloungesite + "/profile/" + ProfileShortID(profile) + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,fr;q=0.6\"; " +
            "\"cookie\"=\"\"} -ContentType \"application/json\"");
                            Debug.WriteLine("[CSGO] Repeat Blocked: " + profile);
                            countblocked += 1;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Equals("{\"code\":\"already_in_black_list\"}"))
                            {
                                Debug.WriteLine("[CSGO] Repeat already in blacklist: " + profile);
                                countalreadyblocked += 1;
                            }
                            else
                            {
                                Debug.WriteLine("[CSGO] Repeat error: " + ex.Message);
                                countunknownerror += 1;
                            }
                        }
                        _general_BlockedLabel.Content = $"Заблокировано: {countblocked}; Пропущено: {countalreadyblocked}; Ошибки: {countunknownerror}";
                        allcounts += 1;
                        RepeatBlockWorker.ReportProgress(allcounts);
                        wait(1);
                    }
                }
            }));
        }

        private void RepeatBlockWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show($"Заблокировано: {countblocked}\nУже в черном списке: {countalreadyblocked}\nОшибки: {countunknownerror}", "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            _general_BlockButton.IsEnabled = true;
            _general_RepeatBlockButton.IsEnabled = true;
        }

        private void _csgo_BlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (_general_BlockUsersListBox.Text != string.Empty)
            {
                _general_BlockButton.IsEnabled = false;
                _general_RepeatBlockButton.IsEnabled = false;
                string[] distinctLines = _general_BlockUsersListBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct().ToArray();
                _general_BlockUsersListBox.Text = string.Join("\r\n", distinctLines);

                BlockingProgressBar.Value = 0;
                BlockingProgressBar.Maximum = GetProfiles().Count;
                BlockListInfo.Refresh();
                BlockWorker.RunWorkerAsync();
            }
        }

        int countblocked;
        int countalreadyblocked;
        int countunknownerror;

        private void BlockWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BlockingProgressBar.Value = e.ProgressPercentage;
        }

        public static void wait(int milliseconds)
        {
            Timer timer1 = new Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };
            while (timer1.Enabled)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void BlockWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
            {
                countblocked = 0;
                countalreadyblocked = 0;
                countunknownerror = 0;
                int allcounts = 0;

                foreach (var profile in GetProfiles())
                {
                    if (profile.Length != 0)
                    {
                        string prof = Regex.Replace(profile, @"\r\n?|\n", "");

                        try
                        {
                            RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/bans/" + ProfileShortID(profile) + "/\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; " +
            "\"path\"=\"/v1/bans/" + ProfileShortID(profile) + "/\"; " +
            "\"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\"; \"origin\"=\"" + csgoloungesite + "\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36\"; \"accept\"=\"*/*\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; " +
            "\"referer\"=\"" + csgoloungesite + "/profile/" + ProfileShortID(profile) + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,fr;q=0.6\"; " +
            "\"cookie\"=\"\"} -ContentType \"application/json\"");
                            Debug.WriteLine("[CSGO] Blocked: " + profile);
                            if (BlockListInfo.Length != 0)
                            {
                                if (!Blocklist.ReadSections(blocklistpath).Contains(prof))
                                {
                                    Blocklist.WriteValue(prof, "Blocked", "true", blocklistpath);
                                }
                            }
                            else
                            {
                                Blocklist.WriteValue(prof, "Blocked", "true", blocklistpath);
                            }

                            countblocked += 1;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Equals("{\"code\":\"already_in_black_list\"}"))
                            {
                                Debug.WriteLine("[CSGO] Already in blacklist: " + profile);
                                if (BlockListInfo.Length != 0)
                                {
                                    if (!Blocklist.ReadSections(blocklistpath).Contains(prof))
                                    {
                                        Blocklist.WriteValue(prof, "Blocked", "true", blocklistpath);
                                    }
                                }
                                else
                                {
                                    Blocklist.WriteValue(prof, "Blocked", "true", blocklistpath);
                                }

                                countalreadyblocked += 1;
                            }
                            else
                            {
                                Debug.WriteLine("[CSGO] Error: " + ex.Message);
                                countunknownerror += 1;
                            }
                        }
                        _general_BlockedLabel.Content = "Заблокировано: " + countblocked + "; Пропущено: " + countalreadyblocked + "; Ошибки: " + countunknownerror + "";
                        allcounts += 1;
                        BlockWorker.ReportProgress(allcounts);
                        wait(1);
                    }
                }
            }));
        }

        public void BlockWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Заблокировано: " + countblocked + Environment.NewLine + "Уже в черном списке: " + countalreadyblocked + Environment.NewLine + "Ошибки: " + countunknownerror, "Lounge Worker", MessageBoxButton.OK, MessageBoxImage.Information);
            if (countblocked != 0 || countalreadyblocked != 0)
            {
                _general_BlockUsersListBox.Text = "";
            }

            _general_BlockButton.IsEnabled = true;
            _general_RepeatBlockButton.IsEnabled = true;
        }

        private string RunScript(string scriptText)
        {
            Thread.CurrentThread.IsBackground = true;
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace
            runspace.Close();
            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }
            return stringBuilder.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _csgo_AutoBumpTimer = new Timer();
            _csgo_AutoBumpTimer.Interval = 1;
            _csgo_AutoBumpTimer.Enabled = false;
            _csgo_AutoBumpTimer.Tick += new EventHandler(_csgo_AutoBumpTimer_Tick);

            _dota_AutoBumpTimer = new Timer();
            _dota_AutoBumpTimer.Interval = 1;
            _dota_AutoBumpTimer.Enabled = false;
            _dota_AutoBumpTimer.Tick += new EventHandler(_dota_AutoBumpTimer_Tick);

            if (!File.Exists(settingspath))
            {
                Config.Write("cfduid", "", "CSGOLounge_Settings");
                Config.Write("authorization", "", "CSGOLounge_Settings");
                Config.Write("BotPath", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 1 (1-8)
                Config.Write("Trade1Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade1Item8", "", "CSGOLounge_Settings");

                Config.Write("Trade2Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 2 (1-8)
                Config.Write("Trade2Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade2Item8", "", "CSGOLounge_Settings");

                Config.Write("Trade3Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 3 (1-8)
                Config.Write("Trade3Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade3Item8", "", "CSGOLounge_Settings");

                Config.Write("Trade4Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 4 (1-8)
                Config.Write("Trade4Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade4Item8", "", "CSGOLounge_Settings");

                Config.Write("Trade5Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 5 (1-8)
                Config.Write("Trade5Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade5Item8", "", "CSGOLounge_Settings");

                Config.Write("Trade6Item1", "", "CSGOLounge_Settings"); // Предметы на сделку 6 (1-8)
                Config.Write("Trade6Item2", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item3", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item4", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item5", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item6", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item7", "", "CSGOLounge_Settings");
                Config.Write("Trade6Item8", "", "CSGOLounge_Settings");

                Config.Write("Bump1", "", "CSGOLounge_Settings"); // Бамп сделок (1-6)
                Config.Write("Bump2", "", "CSGOLounge_Settings");
                Config.Write("Bump3", "", "CSGOLounge_Settings");
                Config.Write("Bump4", "", "CSGOLounge_Settings");
                Config.Write("Bump5", "", "CSGOLounge_Settings");
                Config.Write("Bump6", "", "CSGOLounge_Settings");

                Config.Write("cfduid", "", "DOTA2Lounge_Settings");
                Config.Write("authorization", "", "DOTA2Lounge_Settings");
                Config.Write("BotPath", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem1", "", "DOTA2Lounge_Settings"); // Предметы на пост (1-8)
                Config.Write("PostItem2", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem3", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem4", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem5", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem6", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem7", "", "DOTA2Lounge_Settings");
                Config.Write("PostItem8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade1Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 1 (1-8)
                Config.Write("Trade1Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade1Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade2Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 2 (1-8)
                Config.Write("Trade2Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade2Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade3Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 3 (1-8)
                Config.Write("Trade3Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade3Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade4Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 4 (1-8)
                Config.Write("Trade4Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade4Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade5Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 5 (1-8)
                Config.Write("Trade5Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade5Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Trade6Item1", "", "DOTA2Lounge_Settings"); // Предметы на сделку 6 (1-8)
                Config.Write("Trade6Item2", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item3", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item4", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item5", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item6", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item7", "", "DOTA2Lounge_Settings");
                Config.Write("Trade6Item8", "", "DOTA2Lounge_Settings");

                Config.Write("Bump1", "", "DOTA2Lounge_Settings"); // Бамп сделок (1-6)
                Config.Write("Bump2", "", "DOTA2Lounge_Settings");
                Config.Write("Bump3", "", "DOTA2Lounge_Settings");
                Config.Write("Bump4", "", "DOTA2Lounge_Settings");
                Config.Write("Bump5", "", "DOTA2Lounge_Settings");
                Config.Write("Bump6", "", "DOTA2Lounge_Settings");

                File.Create(blocklistpath);
            }
            else
            {
                if (!BlockListInfo.Exists)
                {
                    BlockListInfo.Create();
                }
                LoadCSGOSettings();
                LoadDotaSettings();
            }
        }

        private void SendBump(string tradeBoxText)
        {
            RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "/v1/trades/" + tradeBoxText + "/bump\" -Method \"POST\" -Headers @{\"method\"=\"POST\"; \"authority\"=\"old.csgolounge.com\"; \"scheme\"=\"https\"; \"path\"=\"/v1/trades/" + tradeBoxText + "/bump\"; \"authorization\"=\"" + _csgo_AuthorizationBox.Text + "\"; \"origin\"=\"" + csgoloungesite + "\"; \"user-agent\"=\"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15\"; \"dnt\"=\"1\"; \"accept\"=\"/\"; \"sec-fetch-site\"=\"same-origin\"; \"sec-fetch-mode\"=\"cors\"; \"referer\"=\"" + csgoloungesite + "/mytrades\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru,en-US;q=0.9,en;q=0.8\"; \"cookie\"=\"__cfduid=" + _csgo_cfduidBox.Text + "\"} -ContentType \"application/json\"");
        }

        private void CSGLWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("[" + DateTime.Now + "] [CSGO] AutoBumpTimer time is come.");
            if (_csgo_Trade1Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + csgoloungesite + "\" -Method \"POST\"");
                    Thread.Sleep(15000);
                    SendBump(_csgo_Trade1Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 1 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 1 Exception: " + ex.Message); }
            }

            if (_csgo_Trade2Box.Text.Length != 0)
            {
                try
                {
                    SendBump(_csgo_Trade2Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 2 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 2 Exception: " + ex.Message); }
            }

            if (_csgo_Trade3Box.Text.Length != 0)
            {
                try
                {
                    SendBump(_csgo_Trade3Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 3 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 3 Exception: " + ex.Message); }
            }

            if (_csgo_Trade4Box.Text.Length != 0)
            {
                try
                {
                    SendBump(_csgo_Trade4Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 4 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 4 Exception: " + ex.Message); }
            }

            if (_csgo_Trade5Box.Text.Length != 0)
            {
                try
                {
                    SendBump(_csgo_Trade5Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 5 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 5 Exception: " + ex.Message); }
            }

            if (_csgo_Trade6Box.Text.Length != 0)
            {
                try
                {
                    SendBump(_csgo_Trade6Box.Text);
                    Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 6 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [CSGO] Trade 6 Exception: " + ex.Message); }
            }

            _csgo_AutoBumpTimer.Interval = 600000;
            Debug.WriteLine("[" + DateTime.Now + "] [CSGO] AutoBumpTimer done, setting interval to 10mins.");
        }

        private void D2Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("[" + DateTime.Now + "] [DOTA] AutoBumpTimer time is come.");
            if (_dota_Trade1Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade1Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade1Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 1 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 1 Exception: " + ex.Message); }
            }

            if (_dota_Trade2Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade2Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade2Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 2 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 2 Exception: " + ex.Message); }
            }

            if (_dota_Trade3Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade3Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade3Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 3 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 3 Exception: " + ex.Message); }
            }

            if (_dota_Trade4Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade4Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade4Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 4 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 4 Exception: " + ex.Message); }
            }

            if (_dota_Trade5Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade5Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade5Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 5 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 5 Exception: " + ex.Message); }
            }

            if (_dota_Trade6Box.Text.Length != 0)
            {
                try
                {
                    RunScript("Invoke-WebRequest -Uri \"" + dota2loungesite + "/v1/trades/" + _dota_Trade6Box.Text + "/bump\" -Method \"POST\" -Headers @{\"sec-fetch-mode\"=\"cors\"; \"origin\"=\"" + dota2loungesite + "\"; \"accept-encoding\"=\"gzip, deflate, br\"; \"accept-language\"=\"ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\"; \"authorization\"=\"" + _dota_AuthorizationBox.Text + "\"; \"cookie\"=\"__cfduid=" + _dota_cfduidBox.Text + "\"; \"path\"=\"/v1/trades/" + _dota_Trade6Box.Text + "/bump\"; \"user-agent\"=\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.150\"; \"accept\"=\"*/*\"; \"referer\"=\"" + dota2loungesite + "/mytrades\"; \"authority\"=\"old.dota2lounge.com\"; \"scheme\"=\"https\"; \"sec-fetch-site\"=\"same-origin\"; \"method\"=\"POST\"} -ContentType \"application/json\"");
                    Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 6 is not empty, sending request to bump.");
                    wait(1);
                }
                catch (Exception ex) { Debug.WriteLine("[" + DateTime.Now + "] [DOTA] Trade 6 Exception: " + ex.Message); }
            }

            _dota_AutoBumpTimer.Interval = 600000;
            Debug.WriteLine("[" + DateTime.Now + "] [DOTA] AutoBumpTimer done, setting interval to 10mins.");
        }
        private void LoadDotaSettings()
        {
            _dota_cfduidBox.Text = Config.Read("cfduid", "DOTA2Lounge_Settings");
            _dota_AuthorizationBox.Text = Config.Read("authorization", "DOTA2Lounge_Settings");
            _dota_BotPathBox.Text = Config.Read("BotPath", "DOTA2Lounge_Settings");
            if (File.Exists(dota2postpath))
            {
                _dota_PostMessage_TextToPostBox.Text = File.ReadAllText(dota2postpath);
            }

            _dota_PostMessage_Item1Box.Text = Config.Read("PostItem1", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item2Box.Text = Config.Read("PostItem2", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item3Box.Text = Config.Read("PostItem3", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item4Box.Text = Config.Read("PostItem4", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item5Box.Text = Config.Read("PostItem5", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item6Box.Text = Config.Read("PostItem6", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item7Box.Text = Config.Read("PostItem7", "DOTA2Lounge_Settings");
            _dota_PostMessage_Item8Box.Text = Config.Read("PostItem8", "DOTA2Lounge_Settings");

            LoadDOTATrade1();
            LoadDOTATrade2();
            LoadDOTATrade3();
            LoadDOTATrade4();
            LoadDOTATrade5();
            LoadDOTATrade6();

            _dota_Trade1Box.Text = Config.Read("Bump1", "DOTA2Lounge_Settings");
            _dota_Trade2Box.Text = Config.Read("Bump2", "DOTA2Lounge_Settings");
            _dota_Trade3Box.Text = Config.Read("Bump3", "DOTA2Lounge_Settings");
            _dota_Trade4Box.Text = Config.Read("Bump4", "DOTA2Lounge_Settings");
            _dota_Trade5Box.Text = Config.Read("Bump5", "DOTA2Lounge_Settings");
            _dota_Trade6Box.Text = Config.Read("Bump6", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade1()
        {
            if (File.Exists(dota2trade1path))
            {
                _dota_CreateTrade_DescriptionBox.Text = File.ReadAllText(dota2trade1path);
            }

            _dota_CreateTrade_Item1Box.Text = Config.Read("Trade1Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box.Text = Config.Read("Trade1Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box.Text = Config.Read("Trade1Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box.Text = Config.Read("Trade1Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box.Text = Config.Read("Trade1Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box.Text = Config.Read("Trade1Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box.Text = Config.Read("Trade1Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box.Text = Config.Read("Trade1Item8", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade2()
        {
            if (File.Exists(dota2trade2path))
            {
                _dota_CreateTrade_DescriptionBox1.Text = File.ReadAllText(dota2trade2path);
            }

            _dota_CreateTrade_Item1Box1.Text = Config.Read("Trade2Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box1.Text = Config.Read("Trade2Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box1.Text = Config.Read("Trade2Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box1.Text = Config.Read("Trade2Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box1.Text = Config.Read("Trade2Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box1.Text = Config.Read("Trade2Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box1.Text = Config.Read("Trade2Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box1.Text = Config.Read("Trade2Item8", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade3()
        {
            if (File.Exists(dota2trade3path))
            {
                _dota_CreateTrade_DescriptionBox2.Text = File.ReadAllText(dota2trade3path);
            }

            _dota_CreateTrade_Item1Box2.Text = Config.Read("Trade3Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box2.Text = Config.Read("Trade3Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box2.Text = Config.Read("Trade3Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box2.Text = Config.Read("Trade3Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box2.Text = Config.Read("Trade3Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box2.Text = Config.Read("Trade3Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box2.Text = Config.Read("Trade3Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box2.Text = Config.Read("Trade3Item8", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade4()
        {
            if (File.Exists(dota2trade4path))
            {
                _dota_CreateTrade_DescriptionBox3.Text = File.ReadAllText(dota2trade4path);
            }

            _dota_CreateTrade_Item1Box3.Text = Config.Read("Trade4Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box3.Text = Config.Read("Trade4Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box3.Text = Config.Read("Trade4Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box3.Text = Config.Read("Trade4Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box3.Text = Config.Read("Trade4Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box3.Text = Config.Read("Trade4Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box3.Text = Config.Read("Trade4Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box3.Text = Config.Read("Trade4Item8", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade5()
        {
            if (File.Exists(dota2trade5path))
            {
                _dota_CreateTrade_DescriptionBox4.Text = File.ReadAllText(dota2trade5path);
            }

            _dota_CreateTrade_Item1Box4.Text = Config.Read("Trade5Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box4.Text = Config.Read("Trade5Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box4.Text = Config.Read("Trade5Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box4.Text = Config.Read("Trade5Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box4.Text = Config.Read("Trade5Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box4.Text = Config.Read("Trade5Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box4.Text = Config.Read("Trade5Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box4.Text = Config.Read("Trade5Item8", "DOTA2Lounge_Settings");
        }

        private void LoadDOTATrade6()
        {
            if (File.Exists(dota2trade6path))
            {
                _dota_CreateTrade_DescriptionBox5.Text = File.ReadAllText(dota2trade6path);
            }

            _dota_CreateTrade_Item1Box5.Text = Config.Read("Trade6Item1", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item2Box5.Text = Config.Read("Trade6Item2", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item3Box5.Text = Config.Read("Trade6Item3", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item4Box5.Text = Config.Read("Trade6Item4", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item5Box5.Text = Config.Read("Trade6Item5", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item6Box5.Text = Config.Read("Trade6Item6", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item7Box5.Text = Config.Read("Trade6Item7", "DOTA2Lounge_Settings");
            _dota_CreateTrade_Item8Box5.Text = Config.Read("Trade6Item8", "DOTA2Lounge_Settings");
        }

        private void LoadCSGOSettings()
        {
            _csgo_cfduidBox.Text = Config.Read("cfduid", "CSGOLounge_Settings");
            _csgo_AuthorizationBox.Text = Config.Read("authorization", "CSGOLounge_Settings");
            _csgo_BotPathBox.Text = Config.Read("BotPath", "CSGOLounge_Settings");

            LoadCSGOTrade1();
            LoadCSGOTrade2();
            LoadCSGOTrade3();
            LoadCSGOTrade4();
            LoadCSGOTrade5();
            LoadCSGOTrade6();

            _csgo_Trade1Box.Text = Config.Read("Bump1", "CSGOLounge_Settings");
            _csgo_Trade2Box.Text = Config.Read("Bump2", "CSGOLounge_Settings");
            _csgo_Trade3Box.Text = Config.Read("Bump3", "CSGOLounge_Settings");
            _csgo_Trade4Box.Text = Config.Read("Bump4", "CSGOLounge_Settings");
            _csgo_Trade5Box.Text = Config.Read("Bump5", "CSGOLounge_Settings");
            _csgo_Trade6Box.Text = Config.Read("Bump6", "CSGOLounge_Settings");
        }

        private void LoadCSGOTrade1()
        {
            if (File.Exists(csgotrade1path))
            {
                _csgo_DescriptionBox.Text = File.ReadAllText(csgotrade1path);
            }

            _csgo_Item1Box.Text = Config.Read("Trade1Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box.Text.Contains("(star)"))
            {
                _csgo_Item1Box.Text = _csgo_Item1Box.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box.Text = Config.Read("Trade1Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box.Text.Contains("(star)"))
            {
                _csgo_Item2Box.Text = _csgo_Item2Box.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box.Text = Config.Read("Trade1Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box.Text.Contains("(star)"))
            {
                _csgo_Item3Box.Text = _csgo_Item3Box.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box.Text = Config.Read("Trade1Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box.Text.Contains("(star)"))
            {
                _csgo_Item4Box.Text = _csgo_Item4Box.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box.Text = Config.Read("Trade1Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box.Text.Contains("(star)"))
            {
                _csgo_Item5Box.Text = _csgo_Item5Box.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box.Text = Config.Read("Trade1Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box.Text.Contains("(star)"))
            {
                _csgo_Item6Box.Text = _csgo_Item6Box.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box.Text = Config.Read("Trade1Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box.Text.Contains("(star)"))
            {
                _csgo_Item7Box.Text = _csgo_Item7Box.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box.Text = Config.Read("Trade1Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box.Text.Contains("(star)"))
            {
                _csgo_Item8Box.Text = _csgo_Item8Box.Text.Replace("(star)", "★");
            }
        }

        private void LoadCSGOTrade2()
        {
            if (File.Exists(csgotrade2path))
            {
                _csgo_DescriptionBox1.Text = File.ReadAllText(csgotrade2path);
            }

            _csgo_Item1Box1.Text = Config.Read("Trade2Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box1.Text.Contains("(star)"))
            {
                _csgo_Item1Box1.Text = _csgo_Item1Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box1.Text = Config.Read("Trade2Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box1.Text.Contains("(star)"))
            {
                _csgo_Item2Box1.Text = _csgo_Item2Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box1.Text = Config.Read("Trade2Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box1.Text.Contains("(star)"))
            {
                _csgo_Item3Box1.Text = _csgo_Item3Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box1.Text = Config.Read("Trade2Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box1.Text.Contains("(star)"))
            {
                _csgo_Item4Box1.Text = _csgo_Item4Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box1.Text = Config.Read("Trade2Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box1.Text.Contains("(star)"))
            {
                _csgo_Item5Box1.Text = _csgo_Item5Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box1.Text = Config.Read("Trade2Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box1.Text.Contains("(star)"))
            {
                _csgo_Item6Box1.Text = _csgo_Item6Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box1.Text = Config.Read("Trade2Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box1.Text.Contains("(star)"))
            {
                _csgo_Item7Box1.Text = _csgo_Item7Box1.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box1.Text = Config.Read("Trade2Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box1.Text.Contains("(star)"))
            {
                _csgo_Item8Box1.Text = _csgo_Item8Box1.Text.Replace("(star)", "★");
            }
        }

        private void LoadCSGOTrade3()
        {
            if (File.Exists(csgotrade3path))
            {
                _csgo_DescriptionBox2.Text = File.ReadAllText(csgotrade3path);
            }

            _csgo_Item1Box2.Text = Config.Read("Trade3Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box2.Text.Contains("(star)"))
            {
                _csgo_Item1Box2.Text = _csgo_Item1Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box2.Text = Config.Read("Trade3Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box2.Text.Contains("(star)"))
            {
                _csgo_Item2Box2.Text = _csgo_Item2Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box2.Text = Config.Read("Trade3Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box2.Text.Contains("(star)"))
            {
                _csgo_Item3Box2.Text = _csgo_Item3Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box2.Text = Config.Read("Trade3Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box2.Text.Contains("(star)"))
            {
                _csgo_Item4Box2.Text = _csgo_Item4Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box2.Text = Config.Read("Trade3Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box2.Text.Contains("(star)"))
            {
                _csgo_Item5Box2.Text = _csgo_Item5Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box2.Text = Config.Read("Trade3Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box2.Text.Contains("(star)"))
            {
                _csgo_Item6Box2.Text = _csgo_Item6Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box2.Text = Config.Read("Trade3Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box2.Text.Contains("(star)"))
            {
                _csgo_Item7Box2.Text = _csgo_Item7Box2.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box2.Text = Config.Read("Trade3Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box2.Text.Contains("(star)"))
            {
                _csgo_Item8Box2.Text = _csgo_Item8Box2.Text.Replace("(star)", "★");
            }
        }

        private void LoadCSGOTrade4()
        {
            if (File.Exists(csgotrade4path))
            {
                _csgo_DescriptionBox3.Text = File.ReadAllText(csgotrade4path);
            }

            _csgo_Item1Box3.Text = Config.Read("Trade4Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box3.Text.Contains("(star)"))
            {
                _csgo_Item1Box3.Text = _csgo_Item1Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box3.Text = Config.Read("Trade4Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box3.Text.Contains("(star)"))
            {
                _csgo_Item2Box3.Text = _csgo_Item2Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box3.Text = Config.Read("Trade4Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box3.Text.Contains("(star)"))
            {
                _csgo_Item3Box3.Text = _csgo_Item3Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box3.Text = Config.Read("Trade4Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box3.Text.Contains("(star)"))
            {
                _csgo_Item4Box3.Text = _csgo_Item4Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box3.Text = Config.Read("Trade4Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box3.Text.Contains("(star)"))
            {
                _csgo_Item5Box3.Text = _csgo_Item5Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box3.Text = Config.Read("Trade4Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box3.Text.Contains("(star)"))
            {
                _csgo_Item6Box3.Text = _csgo_Item6Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box3.Text = Config.Read("Trade4Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box3.Text.Contains("(star)"))
            {
                _csgo_Item7Box3.Text = _csgo_Item7Box3.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box3.Text = Config.Read("Trade4Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box3.Text.Contains("(star)"))
            {
                _csgo_Item8Box3.Text = _csgo_Item8Box3.Text.Replace("(star)", "★");
            }
        }

        private void LoadCSGOTrade5()
        {
            if (File.Exists(csgotrade5path))
            {
                _csgo_DescriptionBox4.Text = File.ReadAllText(csgotrade5path);
            }

            _csgo_Item1Box4.Text = Config.Read("Trade5Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box4.Text.Contains("(star)"))
            {
                _csgo_Item1Box4.Text = _csgo_Item1Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box4.Text = Config.Read("Trade5Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box4.Text.Contains("(star)"))
            {
                _csgo_Item2Box4.Text = _csgo_Item2Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box4.Text = Config.Read("Trade5Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box4.Text.Contains("(star)"))
            {
                _csgo_Item3Box4.Text = _csgo_Item3Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box4.Text = Config.Read("Trade5Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box4.Text.Contains("(star)"))
            {
                _csgo_Item4Box4.Text = _csgo_Item4Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box4.Text = Config.Read("Trade5Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box4.Text.Contains("(star)"))
            {
                _csgo_Item5Box4.Text = _csgo_Item5Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box4.Text = Config.Read("Trade5Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box4.Text.Contains("(star)"))
            {
                _csgo_Item6Box4.Text = _csgo_Item6Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box4.Text = Config.Read("Trade5Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box4.Text.Contains("(star)"))
            {
                _csgo_Item7Box4.Text = _csgo_Item7Box4.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box4.Text = Config.Read("Trade5Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box4.Text.Contains("(star)"))
            {
                _csgo_Item8Box4.Text = _csgo_Item8Box4.Text.Replace("(star)", "★");
            }
        }

        private void LoadCSGOTrade6()
        {
            if (File.Exists(csgotrade6path))
            {
                _csgo_DescriptionBox5.Text = File.ReadAllText(csgotrade6path);
            }

            _csgo_Item1Box5.Text = Config.Read("Trade6Item1", "CSGOLounge_Settings");
            if (_csgo_Item1Box5.Text.Contains("(star)"))
            {
                _csgo_Item1Box5.Text = _csgo_Item1Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item2Box5.Text = Config.Read("Trade6Item2", "CSGOLounge_Settings");
            if (_csgo_Item2Box5.Text.Contains("(star)"))
            {
                _csgo_Item2Box5.Text = _csgo_Item2Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item3Box5.Text = Config.Read("Trade6Item3", "CSGOLounge_Settings");
            if (_csgo_Item3Box5.Text.Contains("(star)"))
            {
                _csgo_Item3Box5.Text = _csgo_Item3Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item4Box5.Text = Config.Read("Trade6Item4", "CSGOLounge_Settings");
            if (_csgo_Item4Box5.Text.Contains("(star)"))
            {
                _csgo_Item4Box5.Text = _csgo_Item4Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item5Box5.Text = Config.Read("Trade6Item5", "CSGOLounge_Settings");
            if (_csgo_Item5Box5.Text.Contains("(star)"))
            {
                _csgo_Item5Box5.Text = _csgo_Item5Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item6Box5.Text = Config.Read("Trade6Item6", "CSGOLounge_Settings");
            if (_csgo_Item6Box5.Text.Contains("(star)"))
            {
                _csgo_Item6Box5.Text = _csgo_Item6Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item7Box5.Text = Config.Read("Trade6Item7", "CSGOLounge_Settings");
            if (_csgo_Item7Box5.Text.Contains("(star)"))
            {
                _csgo_Item7Box5.Text = _csgo_Item7Box5.Text.Replace("(star)", "★");
            }

            _csgo_Item8Box5.Text = Config.Read("Trade6Item8", "CSGOLounge_Settings");
            if (_csgo_Item8Box5.Text.Contains("(star)"))
            {
                _csgo_Item8Box5.Text = _csgo_Item8Box5.Text.Replace("(star)", "★");
            }
        }

        private void _csgo_cfduidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("cfduid", _csgo_cfduidBox.Text, "CSGOLounge_Settings");
        }

        private void _csgo_AuthorizationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("authorization", _csgo_AuthorizationBox.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Item1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade1Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item1Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box1.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade2Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_DescriptionBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade2path, _csgo_DescriptionBox1.Text);
        }

        private void _csgo_Item1Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box2.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade3Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_DescriptionBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade3path, _csgo_DescriptionBox2.Text);
        }

        private void _csgo_Item1Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box3.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade4Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_DescriptionBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade4path, _csgo_DescriptionBox3.Text);
        }

        private void _csgo_Item1Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box4.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade5Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_DescriptionBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade5path, _csgo_DescriptionBox4.Text);
        }

        private void _csgo_Item1Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item1Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item1", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item2Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item2Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item2", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item3Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item3Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item3", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item4Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item4Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item4", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item5Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item5Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item5", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item6Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item6Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item6", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item7Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item7Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item7", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_Item8Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            string formatstr = _csgo_Item8Box5.Text;
            if (formatstr.Contains("★"))
            {
                formatstr = formatstr.Replace("★", "(star)");
            }

            Config.Write("Trade6Item8", formatstr, "CSGOLounge_Settings");
        }

        private void _csgo_DescriptionBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade6path, _csgo_DescriptionBox5.Text);
        }

        private void _csgo_Trade1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump1", _csgo_Trade1Box.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Trade2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump2", _csgo_Trade2Box.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Trade3Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump3", _csgo_Trade3Box.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Trade4Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump4", _csgo_Trade4Box.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Trade5Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump5", _csgo_Trade5Box.Text, "CSGOLounge_Settings");
        }

        private void _csgo_Trade6Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump6", _csgo_Trade6Box.Text, "CSGOLounge_Settings");
        }

        private void _dota_Trade1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump1", _dota_Trade1Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_Trade2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump2", _dota_Trade2Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_Trade3Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump3", _dota_Trade3Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_Trade4Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump4", _dota_Trade4Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_Trade5Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump5", _dota_Trade5Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_Trade6Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Bump6", _dota_Trade6Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_cfduidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("cfduid", _dota_cfduidBox.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_AuthorizationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("authorization", _dota_AuthorizationBox.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem1", _dota_PostMessage_Item1Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem2", _dota_PostMessage_Item2Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item3Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem3", _dota_PostMessage_Item3Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item4Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem4", _dota_PostMessage_Item4Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item5Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem5", _dota_PostMessage_Item5Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item6Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem6", _dota_PostMessage_Item6Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item7Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem7", _dota_PostMessage_Item7Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_Item8Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("PostItem8", _dota_PostMessage_Item8Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_PostMessage_TextToPostBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2postpath, _dota_PostMessage_TextToPostBox.Text);
        }

        private void _csgo_DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(csgotrade1path, _csgo_DescriptionBox.Text);
        }

        private void _dota_CreateTrade_Item1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item1", _dota_CreateTrade_Item1Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item2", _dota_CreateTrade_Item2Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item3", _dota_CreateTrade_Item3Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item4", _dota_CreateTrade_Item4Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item5", _dota_CreateTrade_Item5Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item6", _dota_CreateTrade_Item6Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item7", _dota_CreateTrade_Item7Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade1Item8", _dota_CreateTrade_Item8Box.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade1path, _dota_CreateTrade_DescriptionBox.Text);
        }

        private void _dota_CreateTrade_Item1Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item1", _dota_CreateTrade_Item1Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item2", _dota_CreateTrade_Item2Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item3", _dota_CreateTrade_Item3Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item4", _dota_CreateTrade_Item4Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item5", _dota_CreateTrade_Item5Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item6", _dota_CreateTrade_Item6Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item7", _dota_CreateTrade_Item7Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade2Item8", _dota_CreateTrade_Item8Box1.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade2path, _dota_CreateTrade_DescriptionBox1.Text);
        }

        private void _dota_CreateTrade_Item1Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item1", _dota_CreateTrade_Item1Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item2", _dota_CreateTrade_Item2Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item3", _dota_CreateTrade_Item3Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item4", _dota_CreateTrade_Item4Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item5", _dota_CreateTrade_Item5Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item6", _dota_CreateTrade_Item6Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item7", _dota_CreateTrade_Item7Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade3Item8", _dota_CreateTrade_Item8Box2.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade3path, _dota_CreateTrade_DescriptionBox2.Text);
        }

        private void _dota_CreateTrade_Item1Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item1", _dota_CreateTrade_Item1Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item2", _dota_CreateTrade_Item2Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item3", _dota_CreateTrade_Item3Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item4", _dota_CreateTrade_Item4Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item5", _dota_CreateTrade_Item5Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item6", _dota_CreateTrade_Item6Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item7", _dota_CreateTrade_Item7Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade4Item8", _dota_CreateTrade_Item8Box3.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade4path, _dota_CreateTrade_DescriptionBox3.Text);
        }

        private void _dota_CreateTrade_Item1Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item1", _dota_CreateTrade_Item1Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item2", _dota_CreateTrade_Item2Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item3", _dota_CreateTrade_Item3Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item4", _dota_CreateTrade_Item4Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item5", _dota_CreateTrade_Item5Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item6", _dota_CreateTrade_Item6Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item7", _dota_CreateTrade_Item7Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade5Item8", _dota_CreateTrade_Item8Box4.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade5path, _dota_CreateTrade_DescriptionBox4.Text);
        }

        private void _dota_CreateTrade_Item1Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item1", _dota_CreateTrade_Item1Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item2Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item2", _dota_CreateTrade_Item2Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item3Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item3", _dota_CreateTrade_Item3Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item4Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item4", _dota_CreateTrade_Item4Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item5Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item5", _dota_CreateTrade_Item5Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item6Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item6", _dota_CreateTrade_Item6Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item7Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item7", _dota_CreateTrade_Item7Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_Item8Box5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Write("Trade6Item8", _dota_CreateTrade_Item8Box5.Text, "DOTA2Lounge_Settings");
        }

        private void _dota_CreateTrade_DescriptionBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteToFile(dota2trade6path, _dota_CreateTrade_DescriptionBox5.Text);
        }

        private List<string> GetProfiles()
        {
            List<string> lines = new List<string>();
            int lineCount = _general_BlockUsersListBox.LineCount;
            for (int line = 0; line < lineCount; line++)
            {
                lines.Add(_general_BlockUsersListBox.GetLineText(line));
            }

            return lines;
        }

        private string ProfileShortID(string ProfileLink)
        {
            string[] GetProfileString = ProfileLink.Split(new char[] { '/' });
            if (GetProfileString.Contains(@"/"))
            {
                GetProfileString.ToString().Replace("/", "");
            }
            GetProfileString[4] = GetProfileString[4].Replace(Environment.NewLine, "");

            return GetProfileString[4].ToString();
        }

        private void WriteToFile(string path, string text)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(text);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить.\nПричина: {ex.Message}");
            }
        }

        private void _csgo_ChangeBotPathBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(fd.SelectedPath + @"\main.py"))
                {
                    _csgo_BotPathBox.Text = fd.SelectedPath;
                    Config.Write("BotPath", fd.SelectedPath, "CSGOLounge_Settings");
                }
                else
                {
                    MessageBoxResult dr = MessageBox.Show("Файл для запуска 'main.py' не существует в данном каталоге." + Environment.NewLine + Environment.NewLine + "Повторить попытку?", "Lounge Worker", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (dr == MessageBoxResult.OK)
                    {
                        _csgo_ChangeBotPathBtn_Click(this, null);
                    }
                }
            }
        }

        private void _dota_ChangeBotPathBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(fd.SelectedPath + @"\main.py"))
                {
                    _dota_BotPathBox.Text = fd.SelectedPath;
                    Config.Write("BotPath", fd.SelectedPath, "DOTA2Lounge_Settings");
                }
                else
                {
                    MessageBoxResult dr = MessageBox.Show("Файл для запуска 'main.py' не существует в данном каталоге." + Environment.NewLine + Environment.NewLine + "Повторить попытку?", "Lounge Worker", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (dr == MessageBoxResult.OK)
                    {
                        _dota_ChangeBotPathBtn_Click(this, null);
                    }
                }
            }
        }

        private void _csgo_StartBotBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_csgo_StartBotBtn.Content.ToString() == "Запустить")
            {
                string path = _csgo_BotPathBox.Text + @"\auth.txt";

                try
                {
                    File.WriteAllText(path, _csgo_AuthorizationBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось сохранить ключ в файл.\nПричина: {ex.Message}", "LongeWorker", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                CSGOProcess = new Process();
                CSGOProcess.StartInfo.FileName = "cmd.exe";
                CSGOProcess.StartInfo.UseShellExecute = true;
                CSGOProcess.StartInfo.Verb = "runas";
                CSGOProcess.StartInfo.Arguments = "/C cd \"" + _csgo_BotPathBox.Text + "\"& python main.py";
                CSGOProcess.Exited += CSGOProcess_Exited;
                CSGOProcess.EnableRaisingEvents = true;
                CSGOProcess.Start();
                _csgo_StartBotBtn.Content = "Отключить";
                _csgo_AutoRestart = true;
            }
            else if (_csgo_StartBotBtn.Content.ToString() == "Отключить")
            {
                _csgo_StartBotBtn.Content = "Запустить";
                _csgo_AutoRestart = false;
                CSGOProcess.CloseMainWindow();
            }
        }

        private void CSGOProcess_Exited(object sender, EventArgs e)
        {
            if (CSGOProcess.ExitCode != 0)
            {
                if (_csgo_AutoRestart)
                {
                    CSGOProcess.Start();
                }
            }
        }

        private void _dota_StartBotBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_dota_StartBotBtn.Content.ToString() == "Запустить")
            {
                string path = _dota_BotPathBox.Text + @"\auth.txt";

                try
                {
                    File.WriteAllText(path, _dota_AuthorizationBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось сохранить ключ в файл.\nПричина: {ex.Message}", "LongeWorker", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DotaProcess = new Process();
                DotaProcess.StartInfo.FileName = "cmd.exe";
                DotaProcess.StartInfo.UseShellExecute = true;
                DotaProcess.StartInfo.Verb = "runas";
                DotaProcess.StartInfo.Arguments = "/C cd \"" + _dota_BotPathBox.Text + "\"& python main.py";
                DotaProcess.Exited += DotaProcess_Exited;
                DotaProcess.EnableRaisingEvents = true;
                DotaProcess.Start();
                _dota_StartBotBtn.Content = "Отключить";
                _dota_AutoRestart = true;
            }
            else if (_dota_StartBotBtn.Content.ToString() == "Отключить")
            {
                _dota_StartBotBtn.Content = "Запустить";
                _dota_AutoRestart = false;
                DotaProcess.CloseMainWindow();
            }
        }

        private void DotaProcess_Exited(object sender, EventArgs e)
        {
            if (_dota_AutoRestart)
            {
                DotaProcess.Start();
            }
        }
    }
}
