using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;

namespace DownloadVideoYouTube
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtResolution.SelectedIndex = 0;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string dummyFileName = "Save Here";
            string formats = "All Videos Files |*.mkv;*.mp4;";

            SaveFileDialog sf = new SaveFileDialog();
            // Feed the dummy name to the save dialog
            sf.FileName = dummyFileName;

            //if (sf.ShowDialog() == DialogResult.OK)
            //{
            //    // Now here's our save folder
            //    string savePath = Path.GetDirectoryName(sf.FileName);


            //    progressBar1.Minimum = 0;
            //    IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(txtURL.Text);
            //    VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(txtResolution.Text));
            //    if (video.RequiresDecryption)
            //        DownloadUrlResolver.DecryptDownloadUrl(video);
             //   VideoDownloader downloader = new VideoDownloader(video, Path.Combine(Application.StartupPath + @"", video.Title + video.VideoExtension));
            //    downloader.DownloadProgressChanged += Downloader_ProgressChanged;
            //    Thread thread = new Thread(() => { downloader.Execute(); }) { IsBackground = true };
            //    thread.Start();
            //}

            IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(txtURL.Text);
            VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(txtResolution.Text));
            if (video.RequiresDecryption)
                DownloadUrlResolver.DecryptDownloadUrl(video);            

            // Feed the dummy name to the save dialog
            sf.FileName = video.Title + video.VideoExtension;
            sf.Filter = formats;

            if (sf.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Minimum = 0;

                VideoDownloader downloader = new VideoDownloader(video, Path.Combine(Application.StartupPath + Path.GetDirectoryName(video.Title + video.VideoExtension), video.Title + video.VideoExtension));
                downloader.DownloadProgressChanged += Downloader_ProgressChanged;
                Thread thread = new Thread(() => { downloader.Execute(); }) { IsBackground = true };
                thread.Start();

                // Now here's our save folder  
            }
        }

        private void Downloader_ProgressChanged(object sender, ProgressEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    progressBar1.Value = (int)e.ProgressPercentage;
                    lblPercentage.Text = $"{string.Format("{0:0.##}", e.ProgressPercentage)}%";
                    progressBar1.Update();
                }));
            }
            catch
            {
                Form1 fr = new Form1();
                fr.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            Howto h = new Howto();
            h.ShowDialog();
        }
    }
}
