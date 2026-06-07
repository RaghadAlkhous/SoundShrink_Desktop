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
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("الملف غير موجود.", filePath);
            }

            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".wav" && ext != ".mp3")
            {
                throw new NotSupportedException("يُقبل حالياً ملفات WAV و MP3 فقط.");
            }

            int originalBitsPerSample = GetOriginalBitsPerSample(filePath, ext);

            int originalSampleRate = GetOriginalSampleRate(filePath, ext);

            _reader = new AudioFileReader(filePath);

            return new AudioFileInfo
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                FileSizeBytes = new FileInfo(filePath).Length,
                Duration = _reader.TotalTime,
                SampleRate = originalSampleRate,  
                Channels = _reader.WaveFormat.Channels,
                BitsPerSample = originalBitsPerSample,  
                AverageBytesPerSecond = _reader.WaveFormat.AverageBytesPerSecond
            };
        }

        private int GetOriginalBitsPerSample(string filePath, string ext)
        {
            try
            {
                if (ext == ".wav")
                {
                    using (var waveReader = new WaveFileReader(filePath))
                    {
                        return waveReader.WaveFormat.BitsPerSample;
                    }
                }
                else if (ext == ".mp3")
                {
                    using (var mp3Reader = new Mp3FileReader(filePath))
                    {
                        return mp3Reader.WaveFormat.BitsPerSample;
                    }
                }
            }
            catch
            {
            }

            return 16; 
        }

        private int GetOriginalSampleRate(string filePath, string ext)
        {
            try
            {
                if (ext == ".wav")
                {
                    using (var waveReader = new WaveFileReader(filePath))
                    {
                        return waveReader.WaveFormat.SampleRate;
                    }
                }
                else if (ext == ".mp3")
                {
                    using (var mp3Reader = new Mp3FileReader(filePath))
                    {
                        return mp3Reader.WaveFormat.SampleRate;
                    }
                }
            }
            catch
            {
            }

            return 44100; 
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