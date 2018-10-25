using System.ServiceProcess;
using System.Timers;

namespace RoyaltyScanService
{
    public partial class MyService : ServiceBase
    {
        private static Timer aTimer;

        public MyService()
        {
            InitializeComponent();

            aTimer = new Timer(86400000);
            aTimer.AutoReset = true;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        protected override void OnStart(string[] args)
        {
            // 先跑一遍，然后set 1天的timer
            CheckAndRun();
            aTimer.Start();
        }

        protected override void OnStop()
        {
            aTimer.Stop();
        }


        public void OnTimedEvent(object sender, ElapsedEventArgs args)
        {
            CheckAndRun();
        }

        public async void CheckAndRun()
        {
            var service = new RoyaltyScanService();
            var result = await service.ScanAndUpload();
        }
    }
}
