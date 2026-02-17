using System;

namespace Secugen_HU20
{
    public class AttendanceRecord
    {
        public string StudentId { get; set; }
        public DateTime MarkedAt { get; set; }

        public string ExamDate { get; set; }
        public int ExamGroup { get; set; }
    }
}
