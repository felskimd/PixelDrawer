﻿using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PixelDrawer.ViewModel
{
    class CreateWindowVM : INotifyPropertyChanged
    {
        private string heightText = "64";
        public string HeightText
        {
            get { return heightText; } 
            set
            {
                heightText = value;
                OnPropertyChanged("HeightText");
            }
        }

        private string widthText = "64";
        public string WidthText
        {
            get { return widthText; }
            set
            {
                widthText = value;
                OnPropertyChanged("WidthText");
            }
        }

        private string titleText = "";
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                OnPropertyChanged("TitleText");
            }
        }

        private RelayCommand? acceptCommand;
        public RelayCommand AcceptCommand
        {
            get
            {
                return acceptCommand ??
                  (acceptCommand = new RelayCommand(obj =>
                  {
                      AcceptCommandExecute(obj as View.CreateWindow);
                  }));
            }
        }

        private void AcceptCommandExecute(View.CreateWindow createWindow)
        {
            int width;
            int height;
            if (widthText == string.Empty) { createWindow.createWindow_Width.BorderBrush = Brushes.Red; return; }
            if (heightText == string.Empty) { createWindow.createWindow_Height.BorderBrush = Brushes.Red; return; }
            if (titleText == string.Empty) { createWindow.createWindow_Title.BorderBrush = Brushes.Red; return; }
            width = int.Parse(widthText);
            height = int.Parse(heightText);
            if (width == 0) width = 1;
            if (height == 0) height = 1;
            var mainWindowVM = Application.Current.MainWindow.DataContext as MainWindowVM;
            mainWindowVM.AddProject(titleText, BitmapFactory.New(width, height));
            createWindow.DialogResult = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
