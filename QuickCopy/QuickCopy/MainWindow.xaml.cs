using QuickCopy.Models;
using QuickCopy.ViewModels;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace QuickCopy
{
	public partial class MainWindow : Window
	{
		private ClipboardManager _clipboardManager;
		private bool _isHidden = true;
		private NotifyIcon _notifyIcon;
		private Rect _workingArea = SystemParameters.WorkArea;
		private MainWindowViewModel _viewModel;

		public MainWindow()
		{
			InitializeComponent();
			Hide();
			_workingArea = SystemParameters.WorkArea;
			ShowActivated = false;
			WindowStyle = WindowStyle.None;
			PositionWindow();
			_viewModel = new MainWindowViewModel();
			DataContext = _viewModel;
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			_notifyIcon = new NotifyIcon();
			_notifyIcon.Click += NotifyIcon_Click;
			_notifyIcon.Icon = new Icon("./Icons/tray.ico");
			_notifyIcon.Visible = true;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			_clipboardManager = new ClipboardManager(this);
			_clipboardManager.ClipboardChanged += ClipboardManager_ClipboardChanged;
		}

		private void ClipboardManager_ClipboardChanged(object sender, EventArgs e)
		{
			if (System.Windows.Clipboard.ContainsText())
			{
				var text = System.Windows.Clipboard.GetText();
				var copy = new TextCopy
				{
					Created = DateTime.Now,
					Id = Guid.NewGuid(),
					Text = text
				};
				_viewModel.Copies.Add(copy);
			}
		}

		private void NotifyIcon_Click(object sender, EventArgs e)
		{
			if (_isHidden)
			{
				AnimateIntoView();
			}

			else
			{
				AnimateOutOfView();
			}
		}

		private void PositionWindow()
		{
			Left = _workingArea.Right - ActualWidth;
			Top = _workingArea.Bottom - ActualHeight;

			if (_isHidden)
				Top += 25;
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			PositionWindow();
		}

		private void AnimateIntoView()
		{
			Show();
			var moveAnimation = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromMilliseconds(266)),
				FillBehavior = FillBehavior.Stop,
				EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut }
			};

			var fadeAnimation = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromMilliseconds(266)),
				FillBehavior = FillBehavior.Stop,
				EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut },
				From = 0.5,
				To = 1
			};
			fadeAnimation.Completed += (s, e) => {
				Opacity = 1;
				_isHidden = false;
				Topmost = true;
			};
			Storyboard.SetTarget(fadeAnimation, this);
			Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));
			Storyboard.SetTarget(moveAnimation, this);
			Storyboard.SetTargetProperty(moveAnimation, new PropertyPath(TopProperty));
			moveAnimation.To = Top - 25;

			var storyboard = new Storyboard
			{
				FillBehavior = FillBehavior.Stop
			};
			storyboard.Children.Add(moveAnimation);
			storyboard.Children.Add(fadeAnimation);
			storyboard.Begin();
		}

		private void AnimateOutOfView()
		{
			var fadeAnimation = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromMilliseconds(50)),
				FillBehavior = FillBehavior.Stop,
				EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut },
				From = 1,
				To = 0.75
			};
			fadeAnimation.Completed += (s, e) => {
				Hide();
				Opacity = 0;
				Top += 25;
				Topmost = false;
				_isHidden = true;
			};
			Storyboard.SetTarget(fadeAnimation, this);
			Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));

			var storyboard = new Storyboard
			{
				FillBehavior = FillBehavior.Stop
			};
			storyboard.Children.Add(fadeAnimation);
			storyboard.Begin();
		}

		private void Window_LostFocus(object sender, RoutedEventArgs e)
		{
			AnimateOutOfView();
		}
	}
}
