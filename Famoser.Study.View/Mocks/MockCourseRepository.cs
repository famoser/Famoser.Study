﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Study.Business.Models;
using Famoser.Study.Business.Repositories.Interfaces;

namespace Famoser.Study.View.Mocks
{
    internal class MockCourseRepository : ICourseRepository
    {
        public ObservableCollection<Course> GetCoursesLazy()
        {
            return new ObservableCollection<Course>()
            {
                new Course()
                {
                    Name = "Software Architecture and Engineering",
                    InfoUrl = new Uri("http://www.vvz.ethz.ch/Vorlesungsverzeichnis/lerneinheitPre.do?lerneinheitId=111927&semkez=2017S&lang=de"),
                    Lecturer = "P.Müller, M.Vechev",
                    Place = "CAB G 61",
                    Lectures = new List<Lecture>()
                    {
                        new Lecture()
                        {
                            DayOfWeek = DayOfWeek.Monday,
                            Lecturer = "P.Müller, M.Vechev",
                            Place = "CAB G 61",
                            StartTime = TimeSpan.FromHours(10),
                            EndTime = TimeSpan.FromHours(11)
                        },
                        new Lecture()
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            Lecturer = "P.Müller, M.Vechev",
                            Place = "CAB G 63",
                            StartTime = TimeSpan.FromHours(14),
                            EndTime = TimeSpan.FromHours(15)
                        }
                    }
                }
            };
        }

#pragma warning disable 1998
        public async Task<bool> SaveCourseAsync(Course course)
        {
            return true;
        }

        public async Task<bool> RemoveCourseAsync(Course course)
        {
            return true;
        }

        public async Task<bool> SyncAsnyc()
        {
            return true;
        }
    }
}