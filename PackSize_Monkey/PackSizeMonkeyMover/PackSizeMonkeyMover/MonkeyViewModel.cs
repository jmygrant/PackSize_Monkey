﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace TestWPFBinding
{
	public class MonkeyViewModel : INotifyPropertyChanged
	{
		public int MonkeyRightCount { get; set; }
		public int MonkeyLeftCount { get; set; }
		public bool IsMonkeyLeftVisible { get; set; }
		public bool IsMonkeyLeftMiddleVisible { get; set; }
		public bool IsMonkeyRightMiddleVisible { get; set; }
		public bool IsMonkeyRightVisible { get; set; }
		public bool IsArrowLeftVisible { get; set; }
		public bool IsArrowRightVisible { get; set; }
		public int MonkeysOnRopeCount { get; set; }
		public int MonkeysToMoveCounter { get; set; }


		public MonkeyViewModel()
		{
			MonkeyRightCount = 0;
			MonkeyLeftCount = 0;
			IsMonkeyLeftVisible = false;
			IsMonkeyLeftMiddleVisible = false;
			IsMonkeyRightMiddleVisible = false;
			IsMonkeyRightVisible = false;
			IsArrowLeftVisible = false;
			IsArrowRightVisible = false;
			MonkeysToMoveCounter = 5;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void AddMonkeyToRight()
		{
			MonkeyRightCount++;
			NotifyPropertyChanged("MonkeyRightCount");
		}

		public void AddMonkeyToLeft()
		{
			MonkeyLeftCount++;
			NotifyPropertyChanged("MonkeyLeftCount");
		}

		public void MoveMonkey()
		{
			//MoveLeft
			if (MonkeyLeftCount > 0 && MonkeyRightCount == 0)
			{
				new Thread(() =>
				{
					Thread.CurrentThread.IsBackground = true;
					while (MonkeysToMoveCounter > 0)
					{
						MoveLeft();
					}
					MonkeysToMoveCounter = 5;
				}).Start();
			}

			//MoveRight
			if (MonkeyRightCount > 0 && MonkeyLeftCount == 0)
			{
				new Thread(() =>
				{
					Thread.CurrentThread.IsBackground = true;
					while (MonkeysToMoveCounter > 0)
					{
						MoveRight();
					}
					MonkeysToMoveCounter = 5;
				}).Start();

			}

		}

		private void MoveRight()
		{
			if (MonkeysToMoveCounter > MonkeyRightCount)
			{
				return;
			}
			IsArrowRightVisible = true;
			NotifyPropertyChanged("IsArrowRightVisible");
			if (MonkeysOnRopeCount <= 3 && MonkeyRightCount >= 0)
			{
				if ((IsRopeClear || IsMonkeyRightVisible == false) && MonkeysOnRopeCount < 3)
				{
					MonkeysOnRopeCount = MonkeysOnRopeCount + 1 <= 3 ? MonkeysOnRopeCount + 1 : MonkeysOnRopeCount;
					if (MonkeysOnRopeCount <= 3 && MonkeysToMoveCounter >= 3)
					{
						IsMonkeyRightVisible = true;
						NotifyPropertyChanged("IsMonkeyRightVisible");
					}
					Thread.Sleep(1000);
					return;
				}
				else if (IsMonkeyRightVisible && IsMonkeyRightMiddleVisible == false)
				{
					IsMonkeyRightMiddleVisible = true;
					IsMonkeyRightVisible = false;
					NotifyPropertyChanged("IsMonkeyRightVisible");
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyRightMiddleVisible && IsMonkeyLeftMiddleVisible == false)
				{
					IsMonkeyLeftMiddleVisible = true;
					IsMonkeyRightMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyLeftMiddleVisible && IsMonkeyLeftVisible == false)
				{
					IsMonkeyLeftVisible = true;
					IsMonkeyLeftMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyLeftVisible)
				{
					IsMonkeyLeftVisible = false;
					MonkeyRightCount = MonkeyRightCount - 1 < 0 ? 0 : MonkeyRightCount - 1;
					MonkeysToMoveCounter = MonkeysToMoveCounter - 1 < 0 ? 0 : MonkeysToMoveCounter - 1;
					MonkeysOnRopeCount = MonkeysOnRopeCount - 1 < 0 ? 0 : MonkeysOnRopeCount - 1;
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					NotifyPropertyChanged("MonkeyRightCount");
					Thread.Sleep(1000);

					if (MonkeysToMoveCounter == 0)
					{
						IsArrowRightVisible = false;
						NotifyPropertyChanged("IsArrowRightVisible");
					}

					return;
				}
			}
			
		}

		private void MoveLeft()
		{
			if (MonkeysToMoveCounter > MonkeyLeftCount)
			{
				return;
			}
			IsArrowLeftVisible = true;
			NotifyPropertyChanged("IsArrowLeftVisible");
			if (MonkeysOnRopeCount <= 3 && MonkeyLeftCount >= 0)
			{
				if ((IsRopeClear || IsMonkeyLeftVisible == false) && MonkeysOnRopeCount < 3)
				{
					MonkeysOnRopeCount = MonkeysOnRopeCount + 1 <= 3 ? MonkeysOnRopeCount + 1 : MonkeysOnRopeCount;
					if (MonkeysOnRopeCount <= 3 && MonkeysToMoveCounter >= 3)
					{
						IsMonkeyLeftVisible = true;
						NotifyPropertyChanged("IsMonkeyLeftVisible");
					}
					Thread.Sleep(1000);
					return;
				}
				else if (IsMonkeyLeftVisible && IsMonkeyLeftMiddleVisible == false)
				{
					IsMonkeyLeftMiddleVisible = true;
					IsMonkeyLeftVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyLeftMiddleVisible && IsMonkeyRightMiddleVisible == false)
				{
					IsMonkeyRightMiddleVisible = true;
					IsMonkeyLeftMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyRightMiddleVisible && IsMonkeyRightVisible == false)
				{
					IsMonkeyRightVisible = true;
					IsMonkeyRightMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					NotifyPropertyChanged("IsMonkeyRightVisible");
					Thread.Sleep(1000);

					return;

				}
				else if (IsMonkeyRightVisible)
				{
					IsMonkeyRightVisible = false;
					MonkeyLeftCount = MonkeyLeftCount - 1 < 0 ? 0 : MonkeyLeftCount - 1;
					MonkeysToMoveCounter = MonkeysToMoveCounter - 1 < 0 ? 0 : MonkeysToMoveCounter - 1;
					MonkeysOnRopeCount = MonkeysOnRopeCount - 1 < 0 ? 0 : MonkeysOnRopeCount - 1;
					NotifyPropertyChanged("IsMonkeyRightVisible");
					NotifyPropertyChanged("MonkeyLeftCount");
					Thread.Sleep(1000);

					if (MonkeysToMoveCounter == 0)
					{
						IsArrowLeftVisible = false;
						NotifyPropertyChanged("IsArrowLeftVisible");
					}

					return;
				}
			}
		}

		public bool IsRopeClear
		{
			get
			{
				return !IsMonkeyLeftVisible && !IsMonkeyLeftMiddleVisible &&
			   !IsMonkeyRightMiddleVisible && !IsMonkeyRightVisible;
			}
		}
	}
}
