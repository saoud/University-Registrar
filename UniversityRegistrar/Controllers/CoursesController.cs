using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Controllers 
  {
    public class CoursesController : Controller
    {
      private readonly UniversityRegistrarContext _db;
      public CoursesController(UniversityRegistrarContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        List<Course> model = _db.Courses.ToList();
        return View(model);
      }

      public ActionResult Create()
      {
        return View();
      }

      [HttpPost]
      public ActionResult Create(Course course, int studentId)
      {
        _db.Courses.Add(course);
        _db.SaveChanges();
        if (studentId != 0)
        {
          _db.CourseStudents.Add(new CourseStudent() { StudentId=studentId, CourseId=course.CourseId });
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      
      public ActionResult Details(int id)
      {
        Course thisCourse = _db.Courses
          .Include(course => course.JoinEntities)
          .ThenInclude(join => join.Student)
          .FirstOrDefault(course => course.CourseId == id);
        return View(thisCourse);
      }

      public ActionResult AddStudent(int id)
      {
        Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
        ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
        return View(thisCourse);
      }

      [HttpPost]
      public ActionResult AddStudent(Course course, int studentId)
      {
        if (studentId != 0)
        {
          _db.CourseStudents.Add(new CourseStudent() { StudentId=studentId, CourseId=course.CourseId });
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
  }