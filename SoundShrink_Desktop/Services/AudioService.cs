using NAudio.Wave;
using SoundShrink_Desktop.Models;
using System;
using System.IO;

namespace SoundShrink_Desktop.Services
{
    public class AudioService
    {
        private AudioFileReader _reader;

        public AudioFileInfo LoadAudio(string filePath)
        {
            // تنظيف أي قارئ سابق
            if (_reader != null)
            {
                _reader.Dispose();
            }

            // التحقق من وجود الملف
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("الملف غير موجود.", filePath);
            }

            // التحقق من نوع الملف
            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".wav" && ext != ".mp3")
            {
                throw new NotSupportedException("يُقبل حالياً ملفات WAV و MP3 فقط.");
            }

            // تحميل الملف
            _reader = new AudioFileReader(filePath);

            // استخراج المعلومات
            return new AudioFileInfo
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                FileSizeBytes = new FileInfo(filePath).Length,
                Duration = _reader.TotalTime,
                SampleRate = _reader.WaveFormat.SampleRate,
                Channels = _reader.WaveFormat.Channels,
                BitsPerSample = _reader.WaveFormat.BitsPerSample,
                AverageBytesPerSecond = _reader.WaveFormat.AverageBytesPerSecond
            };
        }

        public AudioFileReader GetReader()
        {
            return _reader;
        }

        public void CloseReader()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }
    }
}