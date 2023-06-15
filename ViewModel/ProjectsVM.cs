using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PixelDrawer.ViewModel
{
    public class ProjectsVM: INotifyPropertyChanged
    {
        private TestProject? selectedProject;
        public TestProject SelectedProject 
        { 
            get => selectedProject; 
            set
            {
                selectedProject = value;
                SelectedLayer = selectedProject.Layers.First();
                OnPropertyChanged("SelectedProject");
            }
        }

        private TestLayer? selectedLayer;
        public TestLayer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        private TestLayer? scale;
        public TestLayer Scale
        {
            get => scale;
            set
            {
                scale = value;
                OnPropertyChanged("Scale");
            }
        }

        private TestProjects Projects = TestProjects.Current;
        public readonly ObservableCollection<TestProject> ProjectsList;

        public ProjectsVM()
        {
            ProjectsList = Projects.ProjectsList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
