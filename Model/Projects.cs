using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PixelDrawer.Model
{
    public class Projects : INotifyPropertyChanged
    {
        private static readonly Projects instance = new Projects();
        public static Projects Current => instance;

        public readonly ObservableCollection<Project> ProjectsList;

        public Projects()
        {
            ProjectsList = new ObservableCollection<Project>();
        }

        public void AddProject(string title, int width, int height, Color backgroundColor)
        {
            ProjectsList.Add(new Project(title, backgroundColor, width, height));
        }

        public void AddProject(Project project)
        {
            ProjectsList.Add(project);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
