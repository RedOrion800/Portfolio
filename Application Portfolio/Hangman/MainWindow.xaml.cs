using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections;

namespace CSharpFinalHangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class HangmanWord
    {
        string word;
        string hiddenWord;
        string category;

        public HangmanWord(string Word, string Category)
        {
            word = Word;
            setHiddenWord(Word);
            category = Category;
        }

        public void setWord(string Word)
        {
            Word.ToUpper();
            word = Word;
            setHiddenWord(Word);
        }

        public void setHiddenWord(string HiddenWord)
        {
            for(int x = 0; x < HiddenWord.Length; x++)
            {
                if(HiddenWord[x] != ' ')
                {
                    hiddenWord += "_";
                }
                else
                {
                    hiddenWord += " ";
                }
            }

            hiddenWord = hiddenWord.Trim();
        }

        public void setCategory(string Category)
        {
            category = Category;
        }

        public string getWord()
        {
            return word.ToUpper();
        }

        public string getHiddenWord()
        {
            return hiddenWord;
        }

        public string getCategory()
        {
            return category;
        }

    }

    public partial class MainWindow : Window
    {
        string[] hangmanImages = { @"images/hangman1.jpg", 
            @"images/hangman2.jpg",
            @"images/hangman3.jpg",
            @"images/hangman4.jpg",
            @"images/hangman5.jpg",
            @"images/hangman6.jpg",
            @"images/hangman7.jpg",
            @"images/hangman8.jpg",
            @"images/hangman9.jpg",
            @"images/hangman10.jpg",
            @"images/hangman11.jpg"};

        List<HangmanWord> words = new List<HangmanWord>();
        
        Random rand = new Random();
        int randomWord;
        int previousRandom = -1;
        int arrayFiller = 0;
        string word;
        string hiddenWord;
        char[] hiddenWordArray;
        bool repeat;
        bool found;
        int wrongGuesses;
        
        public MainWindow()
        {
            InitializeComponent();
            fillWordsArray();
            startGame();
        }

        public void fillWordsArray()
        {
	    //connect to local database
            string databaseConnection = "Data Source=DESKTOP-JVBBQE7\\SQLEXPRESS;Initial Catalog = Hangman; Integrated Security = True";
            string query = "select * from Hangman";
            SqlConnection connection = new SqlConnection(databaseConnection);
            connection.Open();
            if(connection.State == System.Data.ConnectionState.Open)
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    words.Add(new HangmanWord(reader.GetString(0), reader.GetString(1)));
                    
                    arrayFiller++;
                }

            }
            connection.Close();
        }

        public void startGame()
        {
            //for (int x = 0; x < 100; x++)
            //{
            //Debug.WriteLine(randomWord);
            //}

            do
            {
                repeat = false;
                randomWord = rand.Next(0, words.Count);
                if (randomWord == previousRandom)
                {
                    repeat = true;
                }
                else
                {
                    previousRandom = randomWord;
                }

            } while (repeat);

            aButton.Visibility = Visibility.Visible;
            bButton.Visibility = Visibility.Visible;
            cButton.Visibility = Visibility.Visible;
            dButton.Visibility = Visibility.Visible;
            eButton.Visibility = Visibility.Visible;
            fButton.Visibility = Visibility.Visible;
            gButton.Visibility = Visibility.Visible;
            hButton.Visibility = Visibility.Visible;
            iButton.Visibility = Visibility.Visible;
            jButton.Visibility = Visibility.Visible;
            kButton.Visibility = Visibility.Visible;
            lButton.Visibility = Visibility.Visible;
            mButton.Visibility = Visibility.Visible;
            nButton.Visibility = Visibility.Visible;
            oButton.Visibility = Visibility.Visible;
            pButton.Visibility = Visibility.Visible;
            qButton.Visibility = Visibility.Visible;
            rButton.Visibility = Visibility.Visible;
            sButton.Visibility = Visibility.Visible;
            tButton.Visibility = Visibility.Visible;
            uButton.Visibility = Visibility.Visible;
            vButton.Visibility = Visibility.Visible;
            wButton.Visibility = Visibility.Visible;
            xButton.Visibility = Visibility.Visible;
            yButton.Visibility = Visibility.Visible;
            zButton.Visibility = Visibility.Visible;
            playAgainButton.Visibility = Visibility.Hidden;



            word = words[randomWord].getWord();
            hiddenWord = words[randomWord].getHiddenWord();
            hiddenWordArray = words[randomWord].getHiddenWord().ToCharArray();
            fillWordBlock();
            categoryLabel.Content = "Category: " + words[randomWord].getCategory();
            wrongGuesses = 0;
            hangmanImage.Source = new BitmapImage(new Uri(hangmanImages[wrongGuesses], UriKind.Relative));
        }

        public void fillWordBlock()
        {
            wordBlock.Text = "";
            for(int x = 0; x < hiddenWordArray.Length; x++)
            {
                wordBlock.Text += Convert.ToString(hiddenWordArray[x]) + ' ';
            }
            
        }

        public void checkLetter(char letter)
        {
            found = false;
            for(int x = 0; x < hiddenWordArray.Length; x++)
            {
                if(word[x] == letter)
                {
                    found = true;
                    hiddenWordArray[x] = letter;
                }
            }

            if (found)
            {
                hiddenWord = new string(hiddenWordArray);
                //Debug.WriteLine(hiddenWord);
                fillWordBlock();
            }

            else
            {
                wrongGuesses++;
                hangmanImage.Source = new BitmapImage(new Uri(hangmanImages[wrongGuesses], UriKind.Relative));
            }

            if(wrongGuesses == 10)
            {
                lose();
            }

            else if(hiddenWord.Equals(word)) 
            {
                fillWordBlock();
                win();
            }

        }

        public void win()
        {
            aButton.Visibility = Visibility.Hidden;
            bButton.Visibility = Visibility.Hidden;
            cButton.Visibility = Visibility.Hidden;
            dButton.Visibility = Visibility.Hidden;
            eButton.Visibility = Visibility.Hidden;
            fButton.Visibility = Visibility.Hidden;
            gButton.Visibility = Visibility.Hidden;
            hButton.Visibility = Visibility.Hidden;
            iButton.Visibility = Visibility.Hidden;
            jButton.Visibility = Visibility.Hidden;
            kButton.Visibility = Visibility.Hidden;
            lButton.Visibility = Visibility.Hidden;
            mButton.Visibility = Visibility.Hidden;
            nButton.Visibility = Visibility.Hidden;
            oButton.Visibility = Visibility.Hidden;
            pButton.Visibility = Visibility.Hidden;
            qButton.Visibility = Visibility.Hidden;
            rButton.Visibility = Visibility.Hidden;
            sButton.Visibility = Visibility.Hidden;
            tButton.Visibility = Visibility.Hidden;
            uButton.Visibility = Visibility.Hidden;
            vButton.Visibility = Visibility.Hidden;
            wButton.Visibility = Visibility.Hidden;
            xButton.Visibility = Visibility.Hidden;
            yButton.Visibility = Visibility.Hidden;
            zButton.Visibility = Visibility.Hidden;
            playAgainButton.Visibility = Visibility.Visible;

            categoryLabel.Content = "You Win!!!  The answer was " + word;
        }

        public void lose()
        {
            aButton.Visibility = Visibility.Hidden;
            bButton.Visibility = Visibility.Hidden;
            cButton.Visibility = Visibility.Hidden;
            dButton.Visibility = Visibility.Hidden;
            eButton.Visibility = Visibility.Hidden;
            fButton.Visibility = Visibility.Hidden;
            gButton.Visibility = Visibility.Hidden;
            hButton.Visibility = Visibility.Hidden;
            iButton.Visibility = Visibility.Hidden;
            jButton.Visibility = Visibility.Hidden;
            kButton.Visibility = Visibility.Hidden;
            lButton.Visibility = Visibility.Hidden;
            mButton.Visibility = Visibility.Hidden;
            nButton.Visibility = Visibility.Hidden;
            oButton.Visibility = Visibility.Hidden;
            pButton.Visibility = Visibility.Hidden;
            qButton.Visibility = Visibility.Hidden;
            rButton.Visibility = Visibility.Hidden;
            sButton.Visibility = Visibility.Hidden;
            tButton.Visibility = Visibility.Hidden;
            uButton.Visibility = Visibility.Hidden;
            vButton.Visibility = Visibility.Hidden;
            wButton.Visibility = Visibility.Hidden;
            xButton.Visibility = Visibility.Hidden;
            yButton.Visibility = Visibility.Hidden;
            zButton.Visibility = Visibility.Hidden;
            playAgainButton.Visibility = Visibility.Visible;

            categoryLabel.Content = "Game Over!!!  The correct answer was " + word;
        }

        private void aButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(aButton.Content));
            aButton.Visibility = Visibility.Hidden;
        }

        private void bButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(bButton.Content));
            bButton.Visibility = Visibility.Hidden;
        }

        private void cButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(cButton.Content));
            cButton.Visibility = Visibility.Hidden;
        }

        private void dButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(dButton.Content));
            dButton.Visibility = Visibility.Hidden;
        }

        private void eButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(eButton.Content));
            eButton.Visibility = Visibility.Hidden;
        }

        private void fButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(fButton.Content));
            fButton.Visibility = Visibility.Hidden;
        }

        private void gButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(gButton.Content));
            gButton.Visibility = Visibility.Hidden;
        }

        private void hButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(hButton.Content));
            hButton.Visibility = Visibility.Hidden;
        }

        private void iButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(iButton.Content));
            iButton.Visibility = Visibility.Hidden;
        }

        private void jButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(jButton.Content));
            jButton.Visibility = Visibility.Hidden;
        }

        private void kButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(kButton.Content));
            kButton.Visibility = Visibility.Hidden;
        }

        private void lButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(lButton.Content));
            lButton.Visibility = Visibility.Hidden;
        }

        private void mButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(mButton.Content));
            mButton.Visibility = Visibility.Hidden;
        }

        private void nButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(nButton.Content));
            nButton.Visibility = Visibility.Hidden;
        }

        private void oButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(oButton.Content));
            oButton.Visibility = Visibility.Hidden;
        }

        private void pButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(pButton.Content));
            pButton.Visibility = Visibility.Hidden;
        }

        private void qButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(qButton.Content));
            qButton.Visibility = Visibility.Hidden;
        }

        private void rButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(rButton.Content));
            rButton.Visibility = Visibility.Hidden;
        }

        private void sButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(sButton.Content));
            sButton.Visibility = Visibility.Hidden;
        }

        private void tButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(tButton.Content));
            tButton.Visibility = Visibility.Hidden;
        }

        private void uButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(uButton.Content));
            uButton.Visibility = Visibility.Hidden;
        }

        private void vButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(vButton.Content));
            vButton.Visibility = Visibility.Hidden;
        }

        private void wButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(wButton.Content));
            wButton.Visibility = Visibility.Hidden;
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(xButton.Content));
            xButton.Visibility = Visibility.Hidden;
        }

        private void yButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(yButton.Content));
            yButton.Visibility = Visibility.Hidden;
        }

        private void zButton_Click(object sender, RoutedEventArgs e)
        {
            checkLetter(Convert.ToChar(zButton.Content));
            zButton.Visibility = Visibility.Hidden;
        }

        private void playAgainButton_Click(object sender, RoutedEventArgs e)
        {
            startGame();
        }
    }
}
