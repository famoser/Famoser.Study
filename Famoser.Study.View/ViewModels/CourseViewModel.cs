﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using Famoser.FrameworkEssentials.View.Interfaces;
using Famoser.Study.Business.Models;
using Famoser.Study.Business.Repositories.Interfaces;
using Famoser.Study.View.Enum;
using Famoser.Study.View.Services;
using Famoser.Study.View.Services.Interfaces;
using Famoser.Study.View.ViewModels.Base;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace Famoser.Study.View.ViewModels
{
    public class CourseViewModel : BaseViewModel, INavigationBackNotifier
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IHistoryNavigationService _navigationService;
        private readonly IInteractionService _interactionService;
        private readonly IWeekDayService _weekDayService;

        public CourseViewModel(ICourseRepository courseRepository, IHistoryNavigationService navigationService, IInteractionService interactionService, IWeekDayService weekDayService)
        {
            _courseRepository = courseRepository;
            _navigationService = navigationService;
            _interactionService = interactionService;
            _weekDayService = weekDayService;
            Messenger.Default.Register<Course>(this, Messages.Select, SelectCourse);
            if (IsInDesignModeStatic)
            {
                Course = courseRepository.GetCoursesLazy().FirstOrDefault();
            }
        }

        private Course _course;
        public Course Course
        {
            get { return _course; }
            set { Set(ref _course, value); }
        }

        private void SelectCourse(Course obj)
        {
            Course = obj;
        }

        public ICommand SaveCourseCommand => new LoadingRelayCommand(() =>
        {
            _courseRepository.SaveCourseAsync(Course);
            _navigationService.GoBack();
        });

        public ICommand EditCourseCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.AddEditCourse.ToString());
        });

        public ICommand DeleteCourseCommand => new LoadingRelayCommand(async () =>
        {
            if (await _interactionService.ConfirmMessage("do you really want to delete this course?"))
            {
                await _courseRepository.RemoveCourseAsync(Course);
            }
        });

        public ICommand AddLectureCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.AddEditLecture.ToString());
            var lecture = new Lecture
            {
                Lecturer = Course.Lecturer,
                Place = Course.Place,
                Course = Course
            };
            Messenger.Default.Send(lecture, Messages.Select);
        });

        public ICommand EditLectureCommand => new LoadingRelayCommand<Lecture>((l) =>
        {
            _navigationService.NavigateTo(Pages.AddEditLecture.ToString());
            Messenger.Default.Send(l, Messages.Select);
        });

        public ICommand DeleteLectureCommand => new LoadingRelayCommand<Lecture>(async l =>
        {
            if (await _interactionService.ConfirmMessage("do you really want to delete this lecture?"))
            {
                if (Course.Lectures.Contains(l))
                {
                    Course.Lectures.Remove(l);
                    _weekDayService.RefreshCourse(Course);
                    await _courseRepository.SaveCourseAsync(Course);
                }
            }
        });

        public void HandleNavigationBack(object message)
        {
            var back = message as Course;
            if (back != null)
            {
                Course = back;
            }
        }
    }
}
