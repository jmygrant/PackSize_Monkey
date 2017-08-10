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
			else if (MonkeyRightCount > 0 && MonkeyLeftCount == 0)
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
			//This is used for controlling which side to empty first. I was unable to get this working.
			else if(MonkeyLeftCount >= MonkeyRightCount && MonkeyLeftCount > MonkeysToMoveCounter)
			{
				new Thread(() =>
				{
					Thread.CurrentThread.IsBackground = true;
					while (MonkeysToMoveCounter > 0)
					{
						MoveLeft();
					}
					MonkeysToMoveCounter = 5;
					while (MonkeysToMoveCounter > 0)
					{
						MoveRight();
					}
					MonkeysToMoveCounter = 5;
				}).Start();
			}
			else if (MonkeyLeftCount < MonkeyRightCount && MonkeyRightCount > MonkeysToMoveCounter)
			{
				new Thread(() =>
				{
					Thread.CurrentThread.IsBackground = true;
					while (MonkeysToMoveCounter > 0)
					{
						MoveRight();
					}
					MonkeysToMoveCounter = 5;
					while (MonkeysToMoveCounter > 0)
					{
						MoveLeft();
					}
					MonkeysToMoveCounter = 5;
				}).Start();
			}

		}

		/// <summary>
		/// I use the visibility to show the monkeys moving across the line.
		///  I do keep track of how many monkeys are currently on the line and the overall count of monkeys.
		/// </summary>
		private void MoveRight()
		{
			//We do not move monkeys until they have reached a specific threshold.
			if (MonkeysToMoveCounter > MonkeyRightCount)
			{
				return;
			}

			//Makes sure to show and arrow signifing the direction of the monkeys being moved.
			IsArrowRightVisible = true;
			NotifyPropertyChanged("IsArrowRightVisible");

			//First Step: The rope is clear and we add a monkey or the rope is ready to have a monkey.
			if (MonkeysOnRopeCount <= 3 && MonkeyRightCount >= 0)
			{
				//The rope has a limit of 3 monkeys at a time and we make sure that there are monkeys to move.
				if ((IsRopeClear || IsMonkeyRightVisible == false) && MonkeysOnRopeCount < 3)
				{
					//Making sure that we do not add more than 3 monkeys
					MonkeysOnRopeCount = MonkeysOnRopeCount + 1 <= 3 ? MonkeysOnRopeCount + 1 : MonkeysOnRopeCount;
					if (MonkeysOnRopeCount <= 3 && MonkeysToMoveCounter >= 3)
					{
						IsMonkeyRightVisible = true;
						NotifyPropertyChanged("IsMonkeyRightVisible");
					}
					Thread.Sleep(1000);
					return;
				}
				//Step Two: Moving the monkey further right to the 2nd of four positions.
				else if (IsMonkeyRightVisible && IsMonkeyRightMiddleVisible == false)
				{
					IsMonkeyRightMiddleVisible = true;
					IsMonkeyRightVisible = false;
					NotifyPropertyChanged("IsMonkeyRightVisible");
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				//Step Three: Move the monkey further right to the 3rd of four positions.
				else if (IsMonkeyRightMiddleVisible && IsMonkeyLeftMiddleVisible == false)
				{
					IsMonkeyLeftMiddleVisible = true;
					IsMonkeyRightMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					Thread.Sleep(1000);

					return;

				}
				//Step Four: Moving the to the last position when moving Right.
				else if (IsMonkeyLeftMiddleVisible && IsMonkeyLeftVisible == false)
				{
					IsMonkeyLeftVisible = true;
					IsMonkeyLeftMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					Thread.Sleep(1000);

					return;

				}
				//Step Five: The monkey has cleared the rope. Remove it from the rope,
				// the monkey count, and decrement the counter.
				else if (IsMonkeyLeftVisible)
				{
					IsMonkeyLeftVisible = false;
					MonkeyRightCount = MonkeyRightCount - 1 < 0 ? 0 : MonkeyRightCount - 1;
					MonkeysToMoveCounter = MonkeysToMoveCounter - 1 < 0 ? 0 : MonkeysToMoveCounter - 1;
					MonkeysOnRopeCount = MonkeysOnRopeCount - 1 < 0 ? 0 : MonkeysOnRopeCount - 1;
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					NotifyPropertyChanged("MonkeyRightCount");
					Thread.Sleep(1000);

					//Used to hide the arrow so that we do not show one direction movement all the time.
					if (MonkeysToMoveCounter == 0)
					{
						IsArrowRightVisible = false;
						NotifyPropertyChanged("IsArrowRightVisible");
					}

					return;
				}
			}
			
		}

		/// <summary>
		/// I use the visibility to show the monkeys moving across the line.
		///  I do keep track of how many monkeys are currently on the line and the overall count of monkeys.
		/// </summary>
		private void MoveLeft()
		{
			//We do not move monkeys until they have reached a specific threshold.
			if (MonkeysToMoveCounter > MonkeyLeftCount)
			{
				return;
			}

			//Makes sure to show and arrow signifing the direction of the monkeys being moved.
			IsArrowLeftVisible = true;
			NotifyPropertyChanged("IsArrowLeftVisible");

			//The rope has a limit of 3 monkeys at a time and we make sure that there are monkeys to move.
			if (MonkeysOnRopeCount <= 3 && MonkeyLeftCount >= 0)
			{
				//First Step: The rope is clear and we add a monkey or the rope is ready to have a monkey.
				if ((IsRopeClear || IsMonkeyLeftVisible == false) && MonkeysOnRopeCount < 3)
				{
					//Making sure that we do not add more than 3 monkeys
					MonkeysOnRopeCount = MonkeysOnRopeCount + 1 <= 3 ? MonkeysOnRopeCount + 1 : MonkeysOnRopeCount;
					if (MonkeysOnRopeCount <= 3 && MonkeysToMoveCounter >= 3)
					{
						IsMonkeyLeftVisible = true;
						NotifyPropertyChanged("IsMonkeyLeftVisible");
					}
					Thread.Sleep(1000);
					return;
				}
				//Step Two: Moving the monkey further left to the 2nd of four positions.
				else if (IsMonkeyLeftVisible && IsMonkeyLeftMiddleVisible == false)
				{
					IsMonkeyLeftMiddleVisible = true;
					IsMonkeyLeftVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftVisible");
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					Thread.Sleep(1000);

					return;

				}

				//Step Three: Move the monkey further left to the 3rd of four positions.
				else if (IsMonkeyLeftMiddleVisible && IsMonkeyRightMiddleVisible == false)
				{
					IsMonkeyRightMiddleVisible = true;
					IsMonkeyLeftMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyLeftMiddleVisible");
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					Thread.Sleep(1000);

					return;

				}

				//Step Four: Moving the to the last position when moving left.
				else if (IsMonkeyRightMiddleVisible && IsMonkeyRightVisible == false)
				{
					IsMonkeyRightVisible = true;
					IsMonkeyRightMiddleVisible = false;
					NotifyPropertyChanged("IsMonkeyRightMiddleVisible");
					NotifyPropertyChanged("IsMonkeyRightVisible");
					Thread.Sleep(1000);

					return;

				}
				//Step five: The monkey has cleared the rope. Remove it from the rope,
				// the monkey count, and decrement the counter.
				else if (IsMonkeyRightVisible)
				{
					IsMonkeyRightVisible = false;
					MonkeyLeftCount = MonkeyLeftCount - 1 < 0 ? 0 : MonkeyLeftCount - 1;
					MonkeysToMoveCounter = MonkeysToMoveCounter - 1 < 0 ? 0 : MonkeysToMoveCounter - 1;
					MonkeysOnRopeCount = MonkeysOnRopeCount - 1 < 0 ? 0 : MonkeysOnRopeCount - 1;
					NotifyPropertyChanged("IsMonkeyRightVisible");
					NotifyPropertyChanged("MonkeyLeftCount");
					Thread.Sleep(1000);

					//Used to hide the arrow so that we do not show one direction movement all the time.
					if (MonkeysToMoveCounter == 0)
					{
						IsArrowLeftVisible = false;
						NotifyPropertyChanged("IsArrowLeftVisible");
					}

					return;
				}
			}
		}

		//Check to see if there are no monkeys on the rope.
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
