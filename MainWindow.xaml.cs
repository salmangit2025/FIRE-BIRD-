using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;
using System;

namespace Fire_Bird;

public sealed partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new();
    private readonly Random _random = new();
    private double _birdX = 180, _birdY = 140;
    private double _velocityX = 3.0, _velocityY = 1.8;
    private double _aimX = 100, _aimY = 100;
    private int _score, _lives = 3;
    private bool _gameOver;
    private int _frame;

    public MainWindow()
    {
        InitializeComponent();
        Title = "Fire Bird";
        _timer.Interval = TimeSpan.FromMilliseconds(16);
        _timer.Tick += GameTimer_Tick;
        Loaded += MainWindow_Loaded;
        GameCanvas.PointerExited += (_, _) => { };
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        ResetBird();
        UpdateHud();
        _timer.Start();
    }

    private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        KeepBirdInBounds();
        PositionBird();
        PositionAim();
    }

    private void GameTimer_Tick(object? sender, object e)
    {
        if (_gameOver || GameCanvas.ActualWidth < 100 || GameCanvas.ActualHeight < 100)
            return;

        _frame++;
        _birdX += _velocityX;
        _birdY += _velocityY + Math.Sin(_frame / 16.0) * 1.2;

        double maxX = Math.Max(0, GameCanvas.ActualWidth - 64);
        double maxY = Math.Max(40, GameCanvas.ActualHeight - 64);

        if (_birdX <= 0 || _birdX >= maxX)
        {
            _velocityX *= -1;
            _birdX = Math.Clamp(_birdX, 0, maxX);
        }
        if (_birdY <= 35 || _birdY >= maxY)
        {
            _velocityY *= -1;
            _birdY = Math.Clamp(_birdY, 35, maxY);
        }

        PositionBird();
    }

    private void GameCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        Point point = e.GetCurrentPoint(GameCanvas).Position;
        _aimX = point.X;
        _aimY = point.Y;
        PositionAim();
    }

    private void GameCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_gameOver)
            return;

        Point point = e.GetCurrentPoint(GameCanvas).Position;
        _aimX = point.X;
        _aimY = point.Y;
        PositionAim();

        double centerX = _birdX + 24;
        double centerY = _birdY + 24;
        double distance = Math.Sqrt(Math.Pow(point.X - centerX, 2) + Math.Pow(point.Y - centerY, 2));

        if (distance <= 42)
        {
            _score += 10;
            StatusText.Text = "Nice shot! +10 points.";
            ResetBird();
            // The bird gets a little faster as the score increases.
            double speed = Math.Min(7.0, 3.0 + _score / 100.0);
            _velocityX = Math.Sign(_velocityX == 0 ? 1 : _velocityX) * speed;
            _velocityY = Math.Sign(_velocityY == 0 ? 1 : _velocityY) * Math.Min(5.0, 1.8 + _score / 120.0);
            UpdateHud();
        }
        else
        {
            _lives--;
            StatusText.Text = "Missed! Try to lead the moving bird.";
            UpdateHud();
            if (_lives <= 0)
                EndGame();
        }
    }

    private void ResetBird()
    {
        double width = Math.Max(200, GameCanvas.ActualWidth);
        double height = Math.Max(160, GameCanvas.ActualHeight);
        _birdX = _random.Next(20, Math.Max(21, (int)width - 80));
        _birdY = _random.Next(45, Math.Max(46, (int)height - 90));
        _velocityX = _random.Next(0, 2) == 0 ? -3 : 3;
        _velocityY = _random.Next(0, 2) == 0 ? -1.8 : 1.8;
        PositionBird();
    }

    private void KeepBirdInBounds()
    {
        _birdX = Math.Clamp(_birdX, 0, Math.Max(0, GameCanvas.ActualWidth - 64));
        _birdY = Math.Clamp(_birdY, 35, Math.Max(35, GameCanvas.ActualHeight - 64));
    }

    private void PositionBird()
    {
        Canvas.SetLeft(BirdText, _birdX);
        Canvas.SetTop(BirdText, _birdY);
    }

    private void PositionAim()
    {
        Canvas.SetLeft(AimRing, _aimX - 14);
        Canvas.SetTop(AimRing, _aimY - 14);
        AimHorizontal.X1 = _aimX - 10;
        AimHorizontal.X2 = _aimX + 10;
        AimHorizontal.Y1 = _aimY;
        AimHorizontal.Y2 = _aimY;
        AimVertical.X1 = _aimX;
        AimVertical.X2 = _aimX;
        AimVertical.Y1 = _aimY - 10;
        AimVertical.Y2 = _aimY + 10;
    }

    private void UpdateHud()
    {
        ScoreText.Text = _score.ToString();
        LivesText.Text = string.Join(" ", Enumerable.Range(0, 3).Select(i => i < _lives ? "♥" : "♡"));
    }

    private void EndGame()
    {
        _gameOver = true;
        _timer.Stop();
        GameOverText.Visibility = Visibility.Visible;
        Canvas.SetLeft(GameOverText, Math.Max(20, GameCanvas.ActualWidth / 2 - 115));
        Canvas.SetTop(GameOverText, Math.Max(50, GameCanvas.ActualHeight / 2 - 25));
        StatusText.Text = $"Game over! Final score: {_score}. Press Restart Game to play again.";
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        _score = 0;
        _lives = 3;
        _gameOver = false;
        _frame = 0;
        GameOverText.Visibility = Visibility.Collapsed;
        StatusText.Text = "Ready! Hit the bird to score points.";
        ResetBird();
        UpdateHud();
        _timer.Start();
        GameCanvas.Focus(FocusState.Programmatic);
    }
}
