using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Homework_Speech_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> AudioFormatList = new List<string>()
        {
            "PCM","WAV","AMR"
        };
        public MainWindow()
        {
            InitializeComponent();
            audioFormat_CB.ItemsSource = Enum.GetValues(typeof(AudioFormatType)).Cast<AudioFormatType>();
            audioFormat_CB.SelectedValue = AudioFormatType.PCM;
        }

        private void SelectFile_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".pcm";
            dlg.Filter = "Speech Files (*.pcm,*.wav,*.amr)|*.pcm;*.wav;*.amr;*.PCM;*.WAV;*.AMR";
            
            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filePath_TB.Text = dlg.FileName;
            }

        }

        private void StartRecognition_Btn_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            var filePath = this.filePath_TB.Text;
            var audioFormat = (AudioFormatType)audioFormat_CB.SelectedValue;
            if (string.IsNullOrEmpty(filePath))
            {
                flag = false;
                MessageBox.Show("请选择识别文件!");
            }
            else
            {
                int startIndex = filePath.LastIndexOf('.');
                int extlength = filePath.Length - filePath.LastIndexOf('.') - 1;
                var fileExt = filePath.Substring(startIndex + 1, extlength);
                if (audioFormat.ToString().ToLower() != fileExt.ToLower())
                {
                    flag = false;
                    MessageBox.Show("请选择音频格式!");
                }
            }
            if (flag)
            {
                var result = SpeechHelper.AsrData(filePath, audioFormat.ToString().ToLower());
                var speechResult = JsonConvert.DeserializeObject<SpeechModel>(result.ToString());

                //RecognitionResults_RTB.Document.Blocks.Clear();
                speechResult.result.ForEach(item =>
                {
                    Run runx = new Run(item);
                    Paragraph paragraph = new Paragraph(runx);
                    RecognitionResults_RTB.Document.Blocks.Add(paragraph);
                });
            }
        }

        private void StartSynthesis_Btn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(TextToSynthesis_Rtb.Document.ContentStart, TextToSynthesis_Rtb.Document.ContentEnd);
            var result = SpeechHelper.Tts(textRange.Text);
            MessageBox.Show("合成成功！");

        }

    }

    public enum AudioFormatType
    {
        PCM,
        WAV,
        AMR
    }
}
