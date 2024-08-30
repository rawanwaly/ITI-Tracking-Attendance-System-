namespace ITI_Attendance.Services
{
    public interface IProgramService
    {
        public List<ITI_Attendance.Models.Program> GetAll();
        public ITI_Attendance.Models.Program GetById(int id);
    }
}
