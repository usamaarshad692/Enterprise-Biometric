using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Secugen_HU20
{
    public class FileAttendanceStore : IAttendanceStore
    {
        private readonly string filePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance_buffer.txt");

        public void SaveLocal(AttendanceRecord record)
        {
            string line =
                $"{record.StudentId}|{record.ExamDate}|{record.ExamGroup}|{record.MarkedAt:o}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        public List<AttendanceRecord> LoadPending()
        {
            var list = new List<AttendanceRecord>();

            if (!File.Exists(filePath))
                return list;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var p = line.Split('|');
                if (p.Length < 4) continue;

                list.Add(new AttendanceRecord
                {
                    StudentId = p[0],
                    ExamDate = p[1],
                    ExamGroup = int.Parse(p[2]),
                    MarkedAt = DateTime.Parse(p[3])
                });
            }

            return list;
        }

        public void Remove(AttendanceRecord record)
        {
            if (!File.Exists(filePath)) return;

            var remainingLines = File.ReadAllLines(filePath)
                .Where(l => !l.StartsWith(record.StudentId + "|"))
                .ToArray();

            File.WriteAllLines(filePath, remainingLines);
        }
    }


}
