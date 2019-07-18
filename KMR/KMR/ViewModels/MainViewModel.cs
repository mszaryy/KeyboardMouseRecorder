using KMR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMR.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand StartRecord { get; set; }
        public RelayCommand StopRecord { get; set; }

        public RelayCommand StartPlayBackRecord { get; set; }
        public RelayCommand StopPlayBackRecord { get; set; }

        private ObservableCollection<Step> stepsRecorded;
        public ObservableCollection<Step> StepsRecorded
        {
            get { return stepsRecorded; }
            set
            {
                if(value != stepsRecorded)
                {
                    stepsRecorded = value;
                    NotifyPropertyChanged("StepsRecorded");
                }
            }
        }

        private MouseHook mouseHook;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            StepsRecorded = new ObservableCollection<Step>();
            StartRecord = new RelayCommand(StartRecordClick);
            StopRecord = new RelayCommand(StopRecordClick);

            StartPlayBackRecord = new RelayCommand(StartPlayBackRecordClick);
            StopPlayBackRecord = new RelayCommand(StopPlayBackRecordClick);
        }

        private void StopPlayBackRecordClick(object obj)
        {
            throw new NotImplementedException();
        }

        private void StartPlayBackRecordClick(object obj)
        {
            throw new NotImplementedException();
        }

        BackgroundWorker bg;

        private void StopRecordClick(object obj)
        {
             mouseHook.Stop();
             bg.CancelAsync();
        }

        private void StartRecordClick(object obj)
        {
            bg = new BackgroundWorker();
            bg.WorkerSupportsCancellation = true;
            bg.DoWork += Bg_DoWork;
            mouseHook = new MouseHook(StepsRecorded);
            bg.RunWorkerAsync(argument: mouseHook);
        //    System.Threading.Thread.Sleep(5000);

        }


        private static void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            MouseHook mouseHook = e.Argument as MouseHook;
                mouseHook.Start();

            e.Result = mouseHook;
        }

        private static void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MouseHook mouseHook = sender as MouseHook;
            mouseHook.Stop();
        }


        public void NotifyPropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
