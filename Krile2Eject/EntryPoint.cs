using Acuerdo.Plugin;
using Dulcet.Twitter;
using Inscribe.Storage;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading.Tasks;

namespace Krile2Eject
{
    [Export(typeof(IPlugin))]
    public class EntryPoint : IPlugin
    {
        private bool IsOpen = false;

        public IConfigurator ConfigurationInterface
        {
            get { return null; }
        }

        public string Name
        {
            get { return "eject plugin"; }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public void Loaded()
        {
            if (Eject.CanEject())
            {
                TweetStorage.TweetStorageChanged += new EventHandler<TweetStorageChangedEventArgs>(TweetStorage_Changed);
            }
        }

        private void TweetStorage_Changed(object sender, TweetStorageChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (e.ActionKind == TweetActionKind.Added)
                {
                    if (!e.Tweet.IsStatus) return;
                    if (e.Tweet.IsMyTweet) return;
                    if ((e.Tweet.Status as TwitterStatus).RetweetedOriginal != null) return;

                    if (e.Tweet.IsMentionToMe &&
                        e.Tweet.Text.IndexOf("eject", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!this.IsOpen)
                            Eject.Open();
                        else
                            Eject.Close();
                        this.IsOpen = !this.IsOpen;
                    }
                }
            });
        }
    }
}