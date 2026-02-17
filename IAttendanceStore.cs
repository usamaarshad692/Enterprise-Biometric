using System.Collections.Generic;

namespace Secugen_HU20
{
    public interface IAttendanceStore
    {
        void SaveLocal(AttendanceRecord record);
        List<AttendanceRecord> LoadPending();
        void Remove(AttendanceRecord record);
    }

}
