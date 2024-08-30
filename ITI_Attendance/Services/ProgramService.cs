
using ITI_Attendance.Models;

namespace ITI_Attendance.Services
{
    public class ProgramService : IProgramService
    {
        private readonly ITIDbContext db;
        public ProgramService(ITIDbContext _db) => db = _db; 
        public List<Models.Program> GetAll()
        {
            List<Models.Program> programs = db.Programs.ToList();
            return programs;
        }
        public Models.Program GetById(int id) => db.Programs.SingleOrDefault(p => p.Id == id);
    }
}
