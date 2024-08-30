using ITI_Attendance.Models;

namespace ITI_Attendance.Services
{
    public interface IHrService
    {
        public List<Hr> GetAll();
        public Hr GetById (int id);
        public Hr GetByName(string username);
        public Hr ExistingStdEmail(Hr hr, int id);
        public Hr CheckEmail(string Email);
        public void Edit(Hr hr, int id);
        public void Delete(int id);
        public void Add(Hr hr);

    }
}
