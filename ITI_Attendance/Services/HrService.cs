using ITI_Attendance.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI_Attendance.Services
{
    public class HrService : IHrService
    {
        private readonly ITIDbContext db;
        public HrService(ITIDbContext _db) => db = _db;
        public List<Hr> GetAll() => db.Hrs.ToList();
        public Hr GetById(int id) => db.Hrs.SingleOrDefault(h => h.Id == id);
        public Hr GetByName(string username) => db.Hrs.Include(hr => hr.UserLogin).FirstOrDefault(hr => hr.UserLogin.Username == username);
        public Hr ExistingStdEmail(Hr hr,int id) =>db.Hrs.Where(s => s.Email == hr.Email && s.Id != id).FirstOrDefault();
        public Hr CheckEmail(string Email) => db.Hrs.FirstOrDefault(s => s.Email == Email);
        public void Edit(Hr hr, int id)
        {
            var existingHr = db.Hrs.Local.FirstOrDefault(s => s.Id == id);
            if (existingHr != null)
            {
                db.Entry(existingHr).State = EntityState.Detached;
            }

            var hrUpdate = db.Hrs.Find(id);
            if (hrUpdate != null)
            {
                hrUpdate.Name = hr.Name;
                hrUpdate.Email = hr.Email;
                hrUpdate.Age = hr.Age;

                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("HR not found.");
            }
        }
        public void Delete(int id)
        {
            Hr hr = db.Hrs.SingleOrDefault(d => d.Id == id);
            db.Hrs.Remove(hr);
            db.SaveChanges();
        }
        public void Add(Hr hr)
        {
            db.Hrs.Add(hr);
            db.SaveChanges();
        }

    }
}
