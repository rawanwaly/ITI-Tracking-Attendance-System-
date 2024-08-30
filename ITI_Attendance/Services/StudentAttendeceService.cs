using ITI_Attendance.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI_Attendance.Services
{


    public class StudentAttendeceService : IStudentAttendeceService
    {
        private readonly ITIDbContext db;
        public StudentAttendeceService(ITIDbContext _db)
        {
            db = _db;
        }
        public StudentAttendance GetById(int id)
        {
            return db.StudentAttendances.FirstOrDefault(x => x.Id == id);
        }
        public void Add(StudentAttendance st)
        {
            db.StudentAttendances.Add(st);
            db.SaveChanges();
        }
        public void Update(StudentAttendance st)
        {
            Console.WriteLine($"Attempting to update StudentAttendance with Id: {st.Id}");

            var existingEntity = db.StudentAttendances
                .FirstOrDefault(s => s.StudentId == st.Id);

            if (existingEntity != null)
            {
                if (st.Saturday != existingEntity.Saturday)
                {
                    existingEntity.Saturday = true;
                }

                if (st.Sunday != existingEntity.Sunday)
                {
                    existingEntity.Sunday = true;
                }

                if (st.Monday != existingEntity.Monday)
                {
                    existingEntity.Monday = true;
                }

                if (st.Tuesday != existingEntity.Tuesday)
                {
                    existingEntity.Tuesday = true;

                }

                if (st.Wednesday != existingEntity.Wednesday)
                {
                    existingEntity.Wednesday = true;

                }

                if (st.Thursday != existingEntity.Thursday)
                {
                    existingEntity.Thursday = true;

                }

                if (st.Friday != existingEntity.Friday)
                {
                    existingEntity.Friday = true;

                }
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Entity not found");
            }
        }






    }
}
