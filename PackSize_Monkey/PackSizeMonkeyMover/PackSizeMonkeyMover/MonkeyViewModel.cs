using System;
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
				MoveLeft();
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
				}).Start();

			}

			if (MonkeyLeftCount > 0 && MonkeyRightCount > 0)
			{
				Random random = new Random(DateTime.Now.Millisecond);
				var randomValue = random.Next(1, 2);
				if (randomValue % 2 == 0)
				{
					MoveLeft();
				}
				else
				{
					MoveRight();
				}
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
				if ((IsRopeClear || IsMonkeyRightVisible == false) && MonkeyRightCount > 3)
				{
					MonkeysOnRopeCount = MonkeysOnRopeCount + 1 <= 3 ? MonkeysOnRopeCount + 1 : MonkeysOnRopeCount;
					IsMonkeyRightVisible = true;
					NotifyPropertyChanged("IsMonkeyRightVisible");
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

					return;
				}
			}
			IsArrowRightVisible = false;
			NotifyPropertyChanged("IsArrowRightVisible");
			MonkeysToMoveCounter = 5;
		}

		private void MoveLeft()
		{
			if (IsRopeClear)
			{

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
