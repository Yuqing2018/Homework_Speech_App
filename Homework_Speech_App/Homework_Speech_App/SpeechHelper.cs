using Baidu.Aip.Speech;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Homework_Speech_App
{
    public class SpeechHelper
    {
        // 设置APPID/AK/SK
        private const string APP_ID = "14489880";
        private const string API_KEY = "8Sr9u9XYkRPcA62wL02aC43V";
        private const string SECRET_KEY = "i0QtdyawLje2mZEV9kWU5dbQqw8Y76ob";

        private static Asr _asrClient = new Asr(API_KEY, SECRET_KEY) { Timeout = 6000};
        private static Tts _ttsClient = new Tts(API_KEY, SECRET_KEY) { Timeout = 6000 };
        /// <summary>
        /// 通过文件路径，调用百度语音识别接口返回识别结果
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="audioFormat">音频格式</param>
        /// <returns></returns>
        public static JObject AsrData(string filePath,string audioFormat)
        {
            try
            {
                var data = File.ReadAllBytes(filePath);
                _asrClient.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
                var result = _asrClient.Recognize(data, audioFormat, 16000);
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // 合成
        public static bool Tts(string content)
        {
            try
            {
                // 可选参数
                var option = new Dictionary<string, object>()
                {
                    {"spd", 5}, // 语速
                    {"vol", 7}, // 音量
                    {"per", 4}  // 发音人，4：情感度丫丫童声
                };
                var result = _ttsClient.Synthesis(content, option);

                if (result.ErrorCode == 0)  // 或 result.Success
                {
                    SaveFile(result.Data);
                }
                return result.Success;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void SaveFile(byte[] outputBytes)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, outputBytes);
        }

    }

    public class SpeechModel
    {
        public string err_no { get; set; }
        public string err_msg { get; set; }
        public string corpus_no { get; set; }
        public string sn { get; set; }
        public List<string> result { get; set; }
    }
}

