// at the end ::: normalize sound and release timers for mode PCvsPC; set starting with mode Player vs PC; normalize random for draw; arrange the "Others" variables; 
// missings "en passant" for all game modes
// missings "material, repetition and fifty-rule" draws for both PC modes
// materials (for draw) da sledi i za dvata PC moda !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// reduce FullPrint() (at draw, resign, ...)

using System;
using System.IO;
using System.Text;
using System.Security;
using System.Threading;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System.Media;

public class Chessmaster
{
    #region Variables

        #region Console

        #region Size

    public static int consoleWidth = 153;
    public static int consoleHeight = 32;

        #endregion

        #region Colors

    public static ConsoleColor colorOfDefaultBackground = ConsoleColor.Black;
    public static ConsoleColor colorOfDefaultForeground = ConsoleColor.White;

    public static ConsoleColor colorOfOuterFrameBackground = ConsoleColor.DarkGray;
    public static ConsoleColor colorOfOuterFrameForeground = ConsoleColor.White;

    public static ConsoleColor colorOfInternalFrame = ConsoleColor.Black;

    public static ConsoleColor colorOfWhiteFields = ConsoleColor.DarkGray;
    public static ConsoleColor colorOfBlackFields = ConsoleColor.Black;

    public static ConsoleColor colorOfWhiteFigures = ConsoleColor.White;
    public static ConsoleColor colorOfBlackFigures = ConsoleColor.Red;

    public static ConsoleColor colorOfMovesBackground = ConsoleColor.Black;
    public static ConsoleColor colorOfMovesForeground = ConsoleColor.White;

    public static ConsoleColor colorOfTakensWhiteFigureBackground = ConsoleColor.DarkGray;
    public static ConsoleColor colorOfTakensWhiteFigureForeground = ConsoleColor.White;
    public static ConsoleColor colorOfTakensBlackFigureBackground = ConsoleColor.DarkGray;
    public static ConsoleColor colorOfTakensBlackFigureForeground = ConsoleColor.Red;

    public static ConsoleColor colorOfTheEndingMoveBackground = ConsoleColor.Black;
    public static ConsoleColor colorOfTheEndingMoveForeground = ConsoleColor.Red;

    public static ConsoleColor colorOfBottomHelpBackground = ConsoleColor.Black;
    public static ConsoleColor colorOfBottomHelpForeground = ConsoleColor.Yellow;

    //Ivelin
    public static ConsoleColor backgroundColor = ConsoleColor.Black;
    public static ConsoleColor foregroundColorWhileNotChosen = ConsoleColor.Yellow;
    public static ConsoleColor foregroundColorForCurrentMainText = ConsoleColor.Red;
    public static ConsoleColor foregroundColorWhileChosen = ConsoleColor.Green;
    public static ConsoleColor colorForPrinting = ConsoleColor.Green;
    public static ConsoleColor arrows = ConsoleColor.Yellow;
    public static ConsoleColor blueColor = ConsoleColor.Blue;
    public static ConsoleColor colorForFirstText = ConsoleColor.Magenta;
    private static ConsoleColor colorOfOuterFrameBackgroundMenu = ConsoleColor.Blue;
    private static ConsoleColor colorOfOuterFrameForegroundMenu = ConsoleColor.Blue;
    //

        #endregion

        #endregion

        #region Read Move

    public static string gotMove = "";

    public static Queue<char> readMove = new Queue<char>();
    public static string specialFigures = "NBRQK";
    public static string allColumns = "abcdefgh";

    public static string someCastle = "O-O";
    public static string castleKingsideFree = "O-O";
    public static string castleKingsideCheck = "O-O+";
    public static string castleKingsideDraw = "O-O=";
    public static string castleKingsideCheckmate = "O-O#";
    public static string castleQueensideFree = "O-O-O";
    public static string castleQueensideCheck = "O-O-O+";
    public static string castleQueensideDraw = "O-O-O=";
    public static string castleQueensideCheckmate = "O-O-O#";
    public enum Castles { noCastle, castleKingsideFree, castleKingsideCheck, castleKingsideDraw, castleKingsideCheckmate, castleQueensideFree, castleQueensideCheck, castleQueensideDraw, castleQueensideCheckmate };
    public static Castles castle;
    public static int castleRow;
    public static int indexOfCastleRook;
    public static List<List<bool>> castlePossibility = new List<List<bool>>();
    public static bool castleKingside = false;
    public static bool castleQueenside = false;

    public static List<List<List<int>>> savePositions = new List<List<List<int>>>();

    public static bool takes;

    public static States give;
    public static string resign = ".";

    public static string pressed;

    public static string uglyMoveList;
    public static string[] uglyMoveListArray; // zanulqvai go na 2-te mesta!, ako moje

        #endregion

        #region Figure's Units

    public const int whiteDecimals = 10;
    public const int blackDecimals = 30;
    public const int kingUnits = 6;
    public const int queenUnits = 5;
    public const int rookUnits = 4;
    public const int bishopUnits = 3;
    public const int nightUnits = 2;
    public const int pawnUnits = 1;

    public const int whiteKing = whiteDecimals + kingUnits;
    public const int blackKing = blackDecimals + kingUnits;
    public const int whiteRook = whiteDecimals + rookUnits;
    public const int blackRook = blackDecimals + rookUnits;

    public const int whiteKingPlusNight = 2 * whiteDecimals + kingUnits + nightUnits;
    public const int blackKingPlusNight = 2 * blackDecimals + kingUnits + nightUnits;
    public const int whiteKingPlusBishop = 2 * whiteDecimals + kingUnits + bishopUnits;
    public const int blackKingPlusBishop = 2 * blackDecimals + kingUnits + bishopUnits;

    public const int baseMaterial = 8 * pawnUnits + 2 * (nightUnits + bishopUnits + rookUnits) + queenUnits + kingUnits;
    public const int fullWhiteMaterial = 16 * whiteDecimals + baseMaterial;
    public const int fullBlackMaterial = 16 * blackDecimals + baseMaterial;

        #endregion

        #region Battlefield

    public static int[,] battlefield = new int[8, 8];
    public static List<List<List<int>>> figures = new List<List<List<int>>>();
    public static List<int> materials = new List<int>();
    public enum States { Free, Check, Draw, Checkmate, Resign };
    public static List<List<string>> lastMoves = new List<List<string>>();

        #endregion

        #region Players

    public static int playWith = 0;
    public static int oppositePlayWith = 1 - playWith;
    public static string changePlayWith = "c";

    public static int playerToMove = 0;
    public static int oppositePlayer = 1;

    public static States realState = States.Free;

    public static int currentMove = 0;

        #endregion

        #region Talking

    public static StringBuilder sentence = new StringBuilder();

        #endregion

        #region Game modes

    public enum GameModes { PlayerVsComputer = 0, PlayerVsPlayer = 1, ComputerVsComputer = 2, CheckMoveList = 3 };
    public static GameModes gameMode = GameModes.PlayerVsComputer;  // by instancing
    public static string changeGameModeByEventually;
    public static int changeGameModeBy;
    public static bool hadNewGame = false;

    public static List<string> gameModes = new List<string>() { "1", "2", "0", "3" };

        #endregion

        #region Print

    public static int nextWhiteToMove;

    public static List<int> lookingAngles = new List<int>();
    public static int lookingAngle;

    public static string emptyBlackboardRow = new string(' ', 104);

    public static string outerHorizontalEmptyEnds = new string(' ', 4);
    public static string outerHorizontalEmptyMiddle = new string(' ', 40);
    public static string outerHorizontalEmpty = new string(' ', 48);
    public static string outerEmptyByVertical = new string(' ', 3);
    public static List<string> outerHorizontalTextLine = new List<string>() { "      a    b    c    d    e    f    g    h      ",
                                                                              "      1    2    3    4    5    6    7    8      ", 
                                                                              "      h    g    f    e    d    c    b    a      ",
                                                                              "      8    7    6    5    4    3    2    1      " };
    public static string internalHorizontalEmpty = new string(' ', 42);
    public static int outerHorizontalSymbolByVertical;
    public static string fieldEmptyByHorizontal = new string(' ', 5);
    public static int figureInField;

    public static List<string> rotations = new List<string>() { "d", "r", "t", "l" };
    public static int rotate;

    public static bool blindfold = false;
    public static string changeBlindfold = "b";

    public static bool sound = true;
    public static string changeSound = "s";

    public static string bottomHelp = string.Format("Start new game with Players: {0}, {1}, {2}, {3} (check move list)   Change pieces: {4}",
                                                                        gameModes[0], gameModes[1], gameModes[2], gameModes[3], changePlayWith);
    public static string topHelp = string.Format("g5xh6 e7-e8=N+ d2xc1=B= Ra4-h4# Qd4:g1 Kf3xe2 0-0 0-0-0 =? =  Rotations: {0},{1},{2},{3} Blindfold: {4} Sound: {5}", 
                                                                                                rotations[0], rotations[1], rotations[2], rotations[3], changeBlindfold, changeSound);

    public static string playAgain = "Choose and start a new game...";

        #endregion

        #region Timers

    public static DateTime startTime;

    public static int timeBeforeFirstMoveFromList = 1500;
    public static int timeAfterRotationOrBlindfoldChangeBeforeNextMoveFromListOrPC = 1500;
    public static int timeBeforeNextMoveFromList = 2000;
    public static bool hadRotation = false;
    public static bool hadBlindfoldChange = false;

    public static int timeBetweenPrintMoveAndMoveFigure = 500;

    public static int timePCThinksMove = 2000;

        #endregion

        #region Counters

    public static int i;
    public static int j;
    public static int k;

    public static int whitePawn;
    public static int rowOfWhiteSpecialFigure;
    public static int rowOfWhitePawn;
    public static int columnOfWhitePawn;
    public static int blackPawn;
    public static int rowOfBlackSpecialFigure;
    public static int rowOfBlackPawn;
    public static int columnOfBlackPawn;

    public static int ro;
    public static int col;
    public static int mov;
    public static int colon;

        #endregion

        #region Others

    public static bool exchange = false;
    public static int exchangeWith = 0;

    public static bool oppositeCheck;
    public static bool oppositeAvoid;

    public static List<List<int>> lookOnlyThisPositions = new List<List<int>>();
    public static int figureIndex;

    public static bool willBeUnderCheck = false;

    public static int king;
    public static int kingRow;
    public static int kingColumn;
    public static int oppositeKing;
    public static int oppositeKingRow;
    public static int oppositeKingColumn;
    public static int rook;
    public static int theRow;
    public static int rowChange;
    public static int theColumn;
    public static int columnChange;
    public static int figureRow;
    public static int figureColumn;

    public static int theField;

    public static int rowMovement;
    public static int columnMovement;

    public static bool makeNextCheck;

    public static bool canJump;

    public static List<string> moveList = new List<string>();

    public static string choise;
    
    //

    public static int firstOfExchangeKind;

    //public static string giveDraw;
    public static string draw = "=";
    public static string question = "?";
    public static string answer = "=";
    public static string refuse = "-";
    //public static string stalemate = "S";
    //public static string fiftyMove = "F";
    //public static string repetition = "Re";
    //public static string bothFlagsDown = "T";
    //public static string insufficientMaterial = "M";
    public static string drawQuestion = draw + question;
    public static string drawAnswer = answer;
    public static string drawAnswerRefuse = refuse;
    public static string drawByAgreement = draw + question + answer;
    //public static string drawByFiftyMove = draw + fiftyMove;
    //public static string drawByRepetition = draw + repetition;
    //public static string drawByBothFlagsDown = draw + bothFlagsDown;
    //public static string drawByInsufficientMaterial = draw + insufficientMaterial;

    public static List<List<int>> takenFigures = new List<List<int>>();
    public static int[] nextTakenFigure = new int[2] { 0, 0 };
    public static int takenColor;

    public static List<List<List<int>>> possibleMoves = new List<List<List<int>>>();

    public static int toThisField;

    public static int thisKing;
    public static int otherKing;

    public static Random randomGenerator = new Random();
    public static int randomIndex;
    public static int randomOnePossibleMovement;
    public static int randomTakesSymbol;
    public static int randomResign = 1;
    public static int randomResignFrom = 1000;
    public static int randomAcceptDrawByAgreement = 4;
    public static int randomAcceptDrawByAgreementFrom = 100;
    public static int randomOffersDrawByAgreement = 4;
    public static int randomOffersDrawByAgreementFrom = 100;
    public static int randomDrawByMaterial = 100;                   // testing
    public static int randomDrawByMaterialFrom = 100;
    public static int randomDrawByRepetition = 50;
    public static int randomDrawByRepetitionFrom = 100;
    public static int randomDrawByFiftyRule = 50;
    public static int randomDrawByFiftyRuleFrom = 100;

    public static StringBuilder buildTextOfMove = new StringBuilder();

    public static StringBuilder saveCurrentBattlefield = new StringBuilder();
    public static List<string> saveBattlefields = new List<string>();
    public static List<int> matches = new List<int>();
    public static int maxMatches = 0;
    public static int maxMatchesEventually = 0;
    public static int indexOfCurrentBattlefield;

        #endregion

        #region Zeroizing

    public static bool zeroizePrints = false;
    public static bool zeroizeWaits = false;
    public static bool zeroizeSounds = false;

        #endregion

    #endregion

    #region Main function

    public static void Main()
    {
        MenuAtFirstPage();
    }

    # region Menu

    //Play();
    private static void MenuAtFirstPage()
    {
        switch (showHelpOrPlayTheGame())
        {
            case 1:
                do
                {
                    playingSoundWhenStartingTheGame();
                    Play();
                } while (true);
            case 2:
                HelpMenu();
                break;
            default:
                break;
        }
    }

    private static void HelpMenu()
    {
        Console.BackgroundColor = backgroundColor;
        Console.Clear();

        string things = "<   >";
        string numberOne = "1";
        string numberTwo = "2";
        string firtstPage = "Page 1";
        string secondPage = "Page 2";
        string startingGameMessage = "Press Enter for starting your game";
        string[] exampleFirstPage = new string[15]{
            "There are four types of game. To change, you must type the numbers from 0 to 3!",
            "- PC vs. PC (button '0')",
            "- Player vs. PC (button '1')",
            "- Player vs. Player (button '2')",
            "- Check move list (button '3')",
            "\n",
            "Show / Hide figures (blinfold) - button 'b'",
            "Stop / Start sound - button 's'",
            "\n",
            "There are three difficult levels - Amateur, Master, Nightmare.",
            "difficult Amateur - button 'a'",
            "difficult Master - button 'm'",
            "difficult Nightmare - button 'n'",
            "\n",
            "You can rotate the gaming table: 'r'(right), 'l'(left), 't'(top), 'd'(down)",};

        string[] exampleSecondPage = new string[18]{
            "Example: We have a4:b5 ",
            "\n",
            "a - the number of the column that current figure stays on",
            "It can be a, b, c, d, e, f, g, h.",
            "\n",
            "4 - the number of the row that current figure stays on",
            "It can be 1, 2, 3, 4, 5, 6, 7, 8.",
            "\n",
            "':' means that we will get a figure ('x' can also be used).",
            "The enemy's figure that we will get stays on b5.",
            "a4:b5 will move our figure to column 'b' and row 5 and will get",
            "the enemy's figure.",
            "\n",
            "If you want to earn a new figure you must write '=' and the figure you want.", 
            "'=' without special figure after it means that you give a stalemate.", 
            "\n",
            "If you want to give a check you must write '+'. ",
            "For checkmate you must write '#'." };

        //First page
        Console.ForegroundColor = foregroundColorWhileNotChosen;
        Console.SetCursorPosition(74, 1);
        Console.WriteLine(firtstPage);
        Console.ForegroundColor = foregroundColorForCurrentMainText;
        Console.SetCursorPosition(61, 28);
        Console.WriteLine(startingGameMessage);
        Console.ForegroundColor = foregroundColorWhileNotChosen;
        Console.SetCursorPosition(75, 30);
        Console.WriteLine(things);
        Console.SetCursorPosition(76, 30);
        Console.ForegroundColor = foregroundColorWhileChosen;
        Console.WriteLine(numberOne);
        Console.SetCursorPosition(78, 30);
        Console.ForegroundColor = foregroundColorWhileNotChosen;
        Console.WriteLine(numberTwo);

        Console.ForegroundColor = foregroundColorWhileChosen;
        for (int i = 0; i < exampleFirstPage.Length; i++)
        {
            Console.SetCursorPosition(77 - exampleFirstPage[i].Length / 2, i + 3);
            Console.WriteLine(exampleFirstPage[i]);
        }

        int counter = 1;
        ConsoleKeyInfo keyInfo;
        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    do
                    {
                        playingSoundWhenStartingTheGame();
                        Play();
                    } while (true);
                case ConsoleKey.LeftArrow:
                    playingSoundWhenPressingButton();
                    counter--;
                    break;
                case ConsoleKey.RightArrow:
                    playingSoundWhenPressingButton();
                    counter++;
                    break;
            }

            if (counter == 3)
            {
                counter = 1;
            }
            if (counter == 0)
            {
                counter = 2;
            }

            if (counter == 1)
            {
                Console.Clear();
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.SetCursorPosition(74, 1);
                Console.WriteLine(firtstPage);
                Console.ForegroundColor = foregroundColorForCurrentMainText;
                Console.SetCursorPosition(61, 28);
                Console.WriteLine(startingGameMessage);
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.SetCursorPosition(75, 30);
                Console.WriteLine(things);
                Console.SetCursorPosition(76, 30);
                Console.ForegroundColor = foregroundColorWhileChosen;
                Console.WriteLine(numberOne);
                Console.SetCursorPosition(78, 30);
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.WriteLine(numberTwo);

                Console.ForegroundColor = foregroundColorWhileChosen;
                for (int i = 0; i < exampleFirstPage.Length; i++)
                {
                    Console.SetCursorPosition(77 - exampleFirstPage[i].Length / 2, i + 3);
                    Console.WriteLine(exampleFirstPage[i]);
                }
            }

            if (counter == 2)
            {
                Console.Clear();
                Console.SetCursorPosition(74, 1);
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.WriteLine(secondPage);
                Console.ForegroundColor = foregroundColorForCurrentMainText;
                Console.SetCursorPosition(61, 28);
                Console.WriteLine(startingGameMessage);
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.SetCursorPosition(75, 30);
                Console.WriteLine(things);
                Console.SetCursorPosition(76, 30);
                Console.ForegroundColor = foregroundColorWhileNotChosen;
                Console.WriteLine(numberOne);
                Console.SetCursorPosition(78, 30);
                Console.ForegroundColor = foregroundColorWhileChosen;
                Console.WriteLine(numberTwo);

                Console.ForegroundColor = foregroundColorWhileChosen;
                for (int i = 0; i < exampleSecondPage.Length; i++)
                {
                    Console.SetCursorPosition(77 - exampleSecondPage[i].Length / 2, i + 3);
                    Console.WriteLine(exampleSecondPage[i]);
                }
            }
        }
    }

    private static void PrintingOuterFrame()
    {
        Console.SetWindowSize(154, 32);
        Console.Clear();
        Console.SetBufferSize(Console.WindowWidth, 32);
        string outerEmptyByVertical = new string(' ', 3);
        string outerEmptyByHorizontal = new string(' ', Console.BufferWidth);
        string outerEmptyByHorizontalAnotherColor = new string(' ', Console.BufferWidth - 6);
        for (int i = 0; i < Console.BufferHeight - 1; i++)
        {
            Console.BackgroundColor = colorOfOuterFrameBackgroundMenu;
            Console.ForegroundColor = colorOfOuterFrameForegroundMenu;
            Console.SetCursorPosition(0, i);
            Console.WriteLine(outerEmptyByVertical);
            Console.SetCursorPosition(Console.BufferWidth - 3, i);
            Console.WriteLine(outerEmptyByVertical);
        }

        for (int i = 0; i < 2; i++)
        {
            Console.BackgroundColor = colorOfOuterFrameBackgroundMenu;
            Console.ForegroundColor = colorOfOuterFrameForegroundMenu;
            Console.SetCursorPosition(3, i);
            Console.WriteLine(outerEmptyByHorizontal);
        }

        Console.SetCursorPosition(3, 3);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(3, 5);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(3, 2);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(3, 28);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(3, 30);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(0, 30);
        Console.WriteLine(outerEmptyByVertical);
        Console.SetCursorPosition(151, 30);
        Console.WriteLine(outerEmptyByVertical);
        Console.BackgroundColor = colorForPrinting;
        Console.ForegroundColor = colorOfOuterFrameForegroundMenu;
        Console.SetCursorPosition(3, 2);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
        Console.SetCursorPosition(3, 29);
        Console.WriteLine(outerEmptyByHorizontalAnotherColor);
    }

    private static int showHelpOrPlayTheGame()
    {
        Console.SetBufferSize(155, 32);
        Console.CursorVisible = false;
        PrintingOuterFrame();
        string firstText = "Start the game";
        string secondText = "How To Play?";

        Console.BackgroundColor = backgroundColor;
        Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2 - 2);
        Console.ForegroundColor = foregroundColorWhileChosen;
        Console.WriteLine(firstText);
        Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2);
        Console.ForegroundColor = foregroundColorWhileNotChosen;
        Console.WriteLine(secondText);

        int startGameOrShowHelp = 1;
        ConsoleKeyInfo keyInfo;
        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    playingSoundWhenPressingButton();
                    return startGameOrShowHelp;
                case ConsoleKey.UpArrow:
                    playingSoundWhenPressingButton();
                    startGameOrShowHelp--;
                    break;

                case ConsoleKey.DownArrow:
                    playingSoundWhenPressingButton();
                    startGameOrShowHelp++;
                    break;
            }

            switch (startGameOrShowHelp)
            {
                case 1:
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2 - 2);
                    Console.ForegroundColor = foregroundColorWhileChosen;
                    Console.WriteLine(firstText);
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2);
                    Console.ForegroundColor = foregroundColorWhileNotChosen;
                    Console.WriteLine(secondText);
                    break;
                case 2:
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2 - 2);
                    Console.ForegroundColor = foregroundColorWhileNotChosen;
                    Console.WriteLine(firstText);
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2);
                    Console.ForegroundColor = foregroundColorWhileChosen;
                    Console.WriteLine(secondText);
                    break;
                case 0:
                    startGameOrShowHelp = 2;
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2 - 2);
                    Console.ForegroundColor = foregroundColorWhileNotChosen;
                    Console.WriteLine(firstText);
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2);
                    Console.ForegroundColor = foregroundColorWhileChosen;
                    Console.WriteLine(secondText);
                    break;
                case 3:
                    startGameOrShowHelp = 1;
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2 - 2);
                    Console.ForegroundColor = foregroundColorWhileChosen;
                    Console.WriteLine(firstText);
                    Console.SetCursorPosition(Console.BufferWidth / 3 + 19, Console.BufferHeight / 2);
                    Console.ForegroundColor = foregroundColorWhileNotChosen;
                    Console.WriteLine(secondText);
                    break;
                default:
                    break;
            }
        }
        return startGameOrShowHelp;
    }

    private static void playingSoundWhenPressingButton()
    {
        using (SoundPlayer player = new SoundPlayer(@"../../sound.wav"))
        {
            player.Play();
        }
    }

    private static void playingSoundWhenStartingTheGame()
    {
        using (SoundPlayer player = new SoundPlayer(@"../../startingTheGame.wav"))
        {
            player.Play();
        }
    }

    # endregion

    #endregion

    #region Initials

    public static void StartingPosition()
    {
        #region Console

        Console.CursorVisible = false;
        Console.WindowWidth = consoleWidth;
        Console.BufferWidth = consoleWidth;
        Console.WindowHeight = consoleHeight;
        Console.BufferHeight = consoleHeight;
        Console.BackgroundColor = colorOfMovesBackground;
        Console.ForegroundColor = colorOfMovesForeground;
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        #endregion

        #region Game modes

        hadNewGame = false;

        #endregion

        #region Read move

        gotMove = "";
        readMove.Clear();

        takes = false;
        exchange = false;
        exchangeWith = 0;

        give = States.Free;
        realState = States.Free;

        buildTextOfMove.Clear();

        #endregion

        #region Check for check

        willBeUnderCheck = false;

        #endregion

        #region Castle

        castleKingside = false;
        castleQueenside = false;

        #endregion

        #region Players

        playWith = 0;
        oppositePlayWith = 1 - playWith;

        playerToMove = 0;
        oppositePlayer = 1 - playerToMove;

        #endregion

        #region Blindfold

        blindfold = false;

        #endregion

        #region Sound and talking

        sound = true;
        sentence.Clear();

        #endregion

        #region Taken figures

        nextTakenFigure[0] = 0;
        nextTakenFigure[1] = 0;

        #endregion

        #region Save battlefields

        saveCurrentBattlefield.Clear();
        saveBattlefields.Clear();
        matches.Clear();
        maxMatches = 0;
        maxMatchesEventually = 0;

        #endregion

        #region Zeroizing

        zeroizePrints = false;
        zeroizeWaits = false;
        zeroizeSounds = false;

        #endregion

        #region Table

        battlefield[0, 0] = battlefield[0, 7] = whiteDecimals + rookUnits;
        battlefield[0, 1] = battlefield[0, 6] = whiteDecimals + nightUnits;
        battlefield[0, 2] = battlefield[0, 5] = whiteDecimals + bishopUnits;
        battlefield[0, 3] = whiteDecimals + queenUnits;
        battlefield[0, 4] = whiteDecimals + kingUnits;

        for (col = 0; col <= 7; col++)
        {
            battlefield[1, col] = whiteDecimals + pawnUnits;
            battlefield[6, col] = blackDecimals + pawnUnits;
        }

        for (ro = 2; ro <= 5; ro++)
            for (col = 0; col <= 7; col++)
                battlefield[ro, col] = 0;

        battlefield[7, 0] = battlefield[7, 7] = blackDecimals + rookUnits;
        battlefield[7, 1] = battlefield[7, 6] = blackDecimals + nightUnits;
        battlefield[7, 2] = battlefield[7, 5] = blackDecimals + bishopUnits;
        battlefield[7, 3] = blackDecimals + queenUnits;
        battlefield[7, 4] = blackDecimals + kingUnits;

        #endregion

        #region Figures

        figures.Clear();

            #region White

        figures.Add(new List<List<int>>(0));

        figures[0].Add(new List<int>(0));
        figures[0].Add(new List<int>(1));
        figures[0].Add(new List<int>(2));

        figures[0][0].Add(whiteDecimals + kingUnits);
        figures[0][0].Add(whiteDecimals + queenUnits);
        figures[0][0].Add(whiteDecimals + rookUnits);
        figures[0][0].Add(whiteDecimals + rookUnits);
        figures[0][0].Add(whiteDecimals + bishopUnits);
        figures[0][0].Add(whiteDecimals + bishopUnits);
        figures[0][0].Add(whiteDecimals + nightUnits);
        figures[0][0].Add(whiteDecimals + nightUnits);
        for (whitePawn = 0; whitePawn <= 7; whitePawn++)
            figures[0][0].Add(whiteDecimals + pawnUnits);

        for (rowOfWhiteSpecialFigure = 0; rowOfWhiteSpecialFigure <= 7; rowOfWhiteSpecialFigure++)
            figures[0][1].Add(0);
        for (rowOfWhitePawn = 0; rowOfWhitePawn <= 7; rowOfWhitePawn++)
            figures[0][1].Add(1);

        figures[0][2].Add(4);
        figures[0][2].Add(3);
        figures[0][2].Add(0);
        figures[0][2].Add(7);
        figures[0][2].Add(2);
        figures[0][2].Add(5);
        figures[0][2].Add(1);
        figures[0][2].Add(6);
        for (columnOfWhitePawn = 0; columnOfWhitePawn <= 7; columnOfWhitePawn++)
            figures[0][2].Add(columnOfWhitePawn);

            #endregion

            #region Black

        figures.Add(new List<List<int>>(1));

        figures[1].Add(new List<int>(0));
        figures[1].Add(new List<int>(1));
        figures[1].Add(new List<int>(2));

        figures[1][0].Add(blackDecimals + kingUnits);
        figures[1][0].Add(blackDecimals + queenUnits);
        figures[1][0].Add(blackDecimals + rookUnits);
        figures[1][0].Add(blackDecimals + rookUnits);
        figures[1][0].Add(blackDecimals + bishopUnits);
        figures[1][0].Add(blackDecimals + bishopUnits);
        figures[1][0].Add(blackDecimals + nightUnits);
        figures[1][0].Add(blackDecimals + nightUnits);
        for (blackPawn = 0; blackPawn <= 7; blackPawn++)
            figures[1][0].Add(blackDecimals + pawnUnits);

        for (rowOfBlackSpecialFigure = 0; rowOfBlackSpecialFigure <= 7; rowOfBlackSpecialFigure++)
            figures[1][1].Add(7);
        for (rowOfBlackPawn = 0; rowOfBlackPawn <= 7; rowOfBlackPawn++)
            figures[1][1].Add(6);

        figures[1][2].Add(4);
        figures[1][2].Add(3);
        figures[1][2].Add(0);
        figures[1][2].Add(7);
        figures[1][2].Add(2);
        figures[1][2].Add(5);
        figures[1][2].Add(1);
        figures[1][2].Add(6);
        for (columnOfBlackPawn = 0; columnOfBlackPawn <= 7; columnOfBlackPawn++)
            figures[1][2].Add(columnOfBlackPawn);

            #endregion

            #region Materials
        
        materials.Clear();

        materials.Add(fullWhiteMaterial);
        materials.Add(fullBlackMaterial);

            #endregion

            #region Taken figures

        takenFigures.Clear();

        takenFigures.Add(new List<int>());
        takenFigures.Add(new List<int>());

            #endregion

        #endregion

        #region Looking angles

        lookingAngles.Clear();

        lookingAngles.Add(0);
        if (gameMode == GameModes.PlayerVsPlayer)
            lookingAngles.Add(2);
        else
            lookingAngles.Add(0);

        #endregion

        #region Moves

        currentMove = 0;

        moveList.Clear();

        possibleMoves.Clear();

            #region Last moves

        lastMoves.Clear();

        lastMoves.Add(new List<string>(0));
        lastMoves.Add(new List<string>(1));

            #endregion

            #region Save positions

        savePositions.Clear();

        savePositions.Add(new List<List<int>>(0));
        savePositions.Add(new List<List<int>>(1));

        savePositions[0].Add(new List<int>(0));
        savePositions[0].Add(new List<int>(1));

        savePositions[1].Add(new List<int>(0));
        savePositions[1].Add(new List<int>(1));

        savePositions[0][0].Add(0);
        savePositions[0][0].Add(0);
        savePositions[0][0].Add(0);
        savePositions[0][0].Add(0);

        savePositions[0][1].Add(0);
        savePositions[0][1].Add(0);
        savePositions[0][1].Add(0);
        savePositions[0][1].Add(0);

        savePositions[1][0].Add(0);
        savePositions[1][0].Add(0);
        savePositions[1][0].Add(0);
        savePositions[1][0].Add(0);

        savePositions[1][1].Add(0);
        savePositions[1][1].Add(0);
        savePositions[1][1].Add(0);
        savePositions[1][1].Add(0);

            #endregion

            #region Look only this positions
        
        lookOnlyThisPositions.Clear();

        lookOnlyThisPositions.Add(new List<int>(0));
        lookOnlyThisPositions.Add(new List<int>(1));
        lookOnlyThisPositions.Add(new List<int>(2));

            #endregion

            #region Castle

        castlePossibility.Clear();

        castlePossibility.Add(new List<bool>(0));
        castlePossibility.Add(new List<bool>(1));

        castlePossibility[0].Add(true);
        castlePossibility[0].Add(true);
        castlePossibility[0].Add(true);

        castlePossibility[1].Add(true);
        castlePossibility[1].Add(true);
        castlePossibility[1].Add(true);

            #endregion

        #endregion
    }

    public static bool FillMoveList()
    {
        try
        {
            using (StreamReader readRawMoveList = new StreamReader("rawMoveList.txt", Encoding.GetEncoding("windows-1251")))
            {
                uglyMoveList = readRawMoveList.ReadToEnd();
            }

            uglyMoveListArray = uglyMoveList.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            moveList.Clear();
            for (i = 0; i < uglyMoveListArray.Length; i++)
                if (5 <= uglyMoveListArray[i].Length)
                    moveList.Add(uglyMoveListArray[i]);

            using (StreamWriter makeReadyMoveList = new StreamWriter("readyMoveList.txt", false, Encoding.GetEncoding("windows-1251")))
            {
                for (i = 0; i < moveList.Count - 1; i++)
                    makeReadyMoveList.WriteLine(moveList[i]);

                makeReadyMoveList.Write(moveList[moveList.Count - 1]);
            }

            //moveList.Clear();
            //using (StreamReader readReadyMoveList = new StreamReader("readyMoveList.txt", Encoding.GetEncoding("windows-1251")))
            //{
            //    string line;

            //    do
            //    {
            //        line = readReadyMoveList.ReadLine();
            //        if (line == null)
            //            break;

            //        moveList.Add(line);
            //    } while (true);
            //}

            if (moveList.Count < 1)
            {
                Console.WriteLine("The move list file is empty!");
                return false;
            }

            return true;
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("The path to move list file is null!");
            return false;
        }
        catch (ArgumentException)
        {
            Console.WriteLine("The path to move list file is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars!");
            return false;
        }
        catch (PathTooLongException)
        {
            Console.WriteLine("The specified path to move list file, move list file, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters!");
            return false;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("The to move list file specified in path was not found!");
            return false;
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("The specified to move list file is invalid (for example, it is on an unmapped drive)!");
            return false;
        }
        catch (IOException)
        {
            Console.WriteLine("An I/O error occurred while opening a file!");
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("The path specified a file that is read-only.\n"
                            + "-or-\n"
                            + "This operation is not supported on the current platform.\n"
                            + "-or-\n"
                            + "path specified a directory.\n"
                            + "-or- \n"
                            + "The caller does not have the required permission!");
            return false;
        }
        catch (NotSupportedException)
        {
            Console.WriteLine("The path is in an invalid format!");
            return false;
        }
        catch (SecurityException)
        {
            Console.WriteLine("The caller does not have the required permission!");
            return false;
        }
        catch
        {
            Console.WriteLine("Fatal Error!");
            return false;
        }
    }

    #endregion

    #region Print

    public static void FullPrint()
    {
        Console.CursorVisible = false;
        Console.BackgroundColor = colorOfDefaultBackground;
        Console.ForegroundColor = colorOfDefaultForeground;
        Console.Clear();
        PrintBattlefield(lookingAngles[playerToMove]);
        PrintLastMoves();
    }

    public static void PrintBattlefield(int angleOfLooking)
    {
        if (blindfold)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.ForegroundColor = colorOfOuterFrameForeground;
            Console.WriteLine(outerHorizontalEmpty);
            Console.WriteLine(outerHorizontalTextLine[angleOfLooking]);
            Console.WriteLine(outerHorizontalEmpty);
            Console.Write(outerEmptyByVertical);
            Console.BackgroundColor = colorOfInternalFrame;
            Console.Write(internalHorizontalEmpty);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.WriteLine(outerEmptyByVertical);

            for (i = 7; i >= 0; i--)
            {
                switch (angleOfLooking)
                {
                    case 0: { outerHorizontalSymbolByVertical = i + 49; break; }
                    case 1: { outerHorizontalSymbolByVertical = 104 - i; break; }
                    case 2: { outerHorizontalSymbolByVertical = 56 - i; break; }
                    case 3: { outerHorizontalSymbolByVertical = i + 97; break; }
                    default: break;
                }

                for (k = 0; k <= 2; k++)
                {
                    Console.BackgroundColor = colorOfOuterFrameBackground;
                    Console.ForegroundColor = colorOfOuterFrameForeground;
                    Console.Write(" {0} ", k == 1 ? (char)outerHorizontalSymbolByVertical : ' ');
                    Console.BackgroundColor = colorOfInternalFrame;
                    Console.Write(" ");
                    for (j = 0; j <= 7; j++)
                    {
                        if ((i + j + angleOfLooking + 1) % 2 == 0)
                            Console.BackgroundColor = colorOfWhiteFields;
                        else
                            Console.BackgroundColor = colorOfBlackFields;

                        Console.Write(fieldEmptyByHorizontal);
                    }

                    Console.BackgroundColor = colorOfInternalFrame;
                    Console.Write(" ");
                    Console.BackgroundColor = colorOfOuterFrameBackground;
                    Console.ForegroundColor = colorOfOuterFrameForeground;
                    Console.WriteLine(" {0} ", k == 1 ? (char)outerHorizontalSymbolByVertical : ' ');
                }
            }

            Console.Write(outerEmptyByVertical);
            Console.BackgroundColor = colorOfInternalFrame;
            Console.Write(internalHorizontalEmpty);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.WriteLine(outerEmptyByVertical);
            Console.WriteLine(outerHorizontalEmpty);
            Console.WriteLine(outerHorizontalTextLine[angleOfLooking]);
            Console.Write(outerHorizontalEmpty);
        }
        else
        {
            switch (angleOfLooking)
            {
                case 0: { nextTakenFigure[0] = 0; nextTakenFigure[1] = 15; break; }
                case 1: { nextTakenFigure[0] = 15; nextTakenFigure[1] = 0; break; }
                case 2: { nextTakenFigure[0] = 15; nextTakenFigure[1] = 0; break; }
                case 3: { nextTakenFigure[0] = 0; nextTakenFigure[1] = 15; break; }
                default: break;
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.ForegroundColor = colorOfOuterFrameForeground;
            Console.Write(outerHorizontalEmptyEnds);
            switch (angleOfLooking)
            {
                case 1:
                    {
                        for (i = 0; i < 40; i++)
                        {
                            if ((i % 5 != 1) && (i % 5 != 3))
                                Console.Write(" ");
                            else
                            {
                                if (nextTakenFigure[0] < takenFigures[0].Count)
                                {
                                    Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                    Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                    Console.Write(TakenFigure(takenFigures[0][nextTakenFigure[0]]));
                                    Console.BackgroundColor = colorOfOuterFrameBackground;
                                    Console.ForegroundColor = colorOfOuterFrameForeground;
                                }
                                else
                                    Console.Write(" ");

                                nextTakenFigure[0]--;
                            }
                        }

                        break;
                    }
                case 3:
                    {
                        for (i = 0; i < 40; i++)
                        {
                            if ((i % 5 != 1) && (i % 5 != 3))
                                Console.Write(" ");
                            else
                            {
                                if (nextTakenFigure[1] < takenFigures[1].Count)
                                {
                                    Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                    Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                    Console.Write(TakenFigure(takenFigures[1][nextTakenFigure[1]]));
                                    Console.BackgroundColor = colorOfOuterFrameBackground;
                                    Console.ForegroundColor = colorOfOuterFrameForeground;
                                }
                                else
                                    Console.Write(" ");

                                nextTakenFigure[1]--;
                            }
                        }

                        break;
                    }
                default: { Console.Write(outerHorizontalEmptyMiddle); break; }
            }

            Console.WriteLine(outerHorizontalEmptyEnds);
            Console.WriteLine(outerHorizontalTextLine[angleOfLooking]);
            Console.WriteLine(outerHorizontalEmpty);
            Console.Write(outerEmptyByVertical);
            Console.BackgroundColor = colorOfInternalFrame;
            Console.Write(internalHorizontalEmpty);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.WriteLine(outerEmptyByVertical);

            for (i = 7; i >= 0; i--)
            {
                switch (angleOfLooking)
                {
                    case 0: { outerHorizontalSymbolByVertical = i + 49; break; }
                    case 1: { outerHorizontalSymbolByVertical = 104 - i; break; }
                    case 2: { outerHorizontalSymbolByVertical = 56 - i; break; }
                    case 3: { outerHorizontalSymbolByVertical = i + 97; break; }
                    default: break;
                }

                for (k = 0; k <= 2; k++)
                {
                    Console.BackgroundColor = colorOfOuterFrameBackground;
                    Console.ForegroundColor = colorOfOuterFrameForeground;
                    if (k == 1)
                        Console.Write(" ");
                    else
                    {
                        switch (angleOfLooking)
                        {
                            case 0:
                                {
                                    if (nextTakenFigure[0] < takenFigures[0].Count)
                                    {
                                        Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                        Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                        Console.Write(TakenFigure(takenFigures[0][nextTakenFigure[0]]));
                                        Console.BackgroundColor = colorOfOuterFrameBackground;
                                        Console.ForegroundColor = colorOfOuterFrameForeground;
                                    }
                                    else
                                        Console.Write(" ");

                                    nextTakenFigure[0]++;
                                    break;
                                }
                            case 2:
                                {
                                    if (nextTakenFigure[1] < takenFigures[1].Count)
                                    {
                                        Console.BackgroundColor = colorOfTakensBlackFigureBackground;
                                        Console.ForegroundColor = colorOfTakensBlackFigureForeground;
                                        Console.Write(TakenFigure(takenFigures[1][nextTakenFigure[1]]));
                                        Console.BackgroundColor = colorOfOuterFrameBackground;
                                        Console.ForegroundColor = colorOfOuterFrameForeground;
                                    }
                                    else
                                        Console.Write(" ");

                                    nextTakenFigure[1]++;
                                    break;
                                }
                            default: { Console.Write(" "); break; }
                        }
                    }

                    Console.Write("{0} ", k == 1 ? (char)outerHorizontalSymbolByVertical : ' ');
                    Console.BackgroundColor = colorOfInternalFrame;
                    Console.Write(" ");
                    for (j = 0; j <= 7; j++)
                    {
                        switch (angleOfLooking)
                        {
                            case 0: { figureInField = battlefield[i, j]; break; }
                            case 1: { figureInField = battlefield[j, 7 - i]; break; }
                            case 2: { figureInField = battlefield[7 - i, 7 - j]; break; }
                            case 3: { figureInField = battlefield[7 - j, i]; break; }
                            default: break;
                        }

                        if ((i + j + angleOfLooking + 1) % 2 == 0)
                            Console.BackgroundColor = colorOfWhiteFields;
                        else
                            Console.BackgroundColor = colorOfBlackFields;

                        if (k == 1)
                            Console.Write("  {0}  ", Figure(figureInField));
                        else
                            Console.Write(fieldEmptyByHorizontal);
                    }

                    Console.BackgroundColor = colorOfInternalFrame;
                    Console.Write(" ");
                    Console.BackgroundColor = colorOfOuterFrameBackground;
                    Console.ForegroundColor = colorOfOuterFrameForeground;
                    Console.Write(" {0}", k == 1 ? (char)outerHorizontalSymbolByVertical : ' ');
                    if (k == 1)
                        Console.WriteLine(" ");
                    else
                    {
                        switch (angleOfLooking)
                        {
                            case 0:
                                {
                                    if (nextTakenFigure[1] < takenFigures[1].Count)
                                    {
                                        Console.BackgroundColor = colorOfTakensBlackFigureBackground;
                                        Console.ForegroundColor = colorOfTakensBlackFigureForeground;
                                        Console.WriteLine(TakenFigure(takenFigures[1][nextTakenFigure[1]]));
                                        Console.BackgroundColor = colorOfOuterFrameBackground;
                                        Console.ForegroundColor = colorOfOuterFrameForeground;
                                    }
                                    else
                                        Console.WriteLine(" ");

                                    nextTakenFigure[1]--;
                                    break;
                                }
                            case 2:
                                {
                                    if (nextTakenFigure[0] < takenFigures[0].Count)
                                    {
                                        Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                        Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                        Console.WriteLine(TakenFigure(takenFigures[0][nextTakenFigure[0]]));
                                        Console.BackgroundColor = colorOfOuterFrameBackground;
                                        Console.ForegroundColor = colorOfOuterFrameForeground;
                                    }
                                    else
                                        Console.WriteLine(" ");

                                    nextTakenFigure[0]--;
                                    break;
                                }
                            default: { Console.WriteLine(" "); break; }
                        }
                    }
                }
            }

            Console.Write(outerEmptyByVertical);
            Console.BackgroundColor = colorOfInternalFrame;
            Console.Write(internalHorizontalEmpty);
            Console.BackgroundColor = colorOfOuterFrameBackground;
            Console.WriteLine(outerEmptyByVertical);
            Console.WriteLine(outerHorizontalEmpty);
            Console.WriteLine(outerHorizontalTextLine[angleOfLooking]);
            Console.Write(outerHorizontalEmptyEnds);
            switch (angleOfLooking)
            {
                case 1:
                    {
                        for (i = 0; i < 40; i++)
                        {
                            if ((i % 5 != 1) && (i % 5 != 3))
                                Console.Write(" ");
                            else
                            {
                                if (nextTakenFigure[1] < takenFigures[1].Count)
                                {
                                    Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                    Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                    Console.Write(TakenFigure(takenFigures[1][nextTakenFigure[1]]));
                                    Console.BackgroundColor = colorOfOuterFrameBackground;
                                    Console.ForegroundColor = colorOfOuterFrameForeground;
                                }
                                else
                                    Console.Write(" ");

                                nextTakenFigure[1]++;
                            }
                        }

                        break;
                    }
                case 3:
                    {
                        for (i = 0; i < 40; i++)
                        {
                            if ((i % 5 != 1) && (i % 5 != 3))
                                Console.Write(" ");
                            else
                            {
                                if (nextTakenFigure[0] < takenFigures[0].Count)
                                {
                                    Console.BackgroundColor = colorOfTakensWhiteFigureBackground;
                                    Console.ForegroundColor = colorOfTakensWhiteFigureForeground;
                                    Console.Write(TakenFigure(takenFigures[0][nextTakenFigure[0]]));
                                    Console.BackgroundColor = colorOfOuterFrameBackground;
                                    Console.ForegroundColor = colorOfOuterFrameForeground;
                                }
                                else
                                    Console.Write(" ");

                                nextTakenFigure[0]++;
                            }
                        }

                        break;
                    }
                default: { Console.Write(outerHorizontalEmptyMiddle); break; }
            }

            Console.Write(outerHorizontalEmptyEnds);
        }
    }

    public static char Figure(int field)
    {
        if ((figures[0][0][0] - kingUnits < field) && (field <= figures[0][0][0]))
            Console.ForegroundColor = colorOfWhiteFigures;
        else
            Console.ForegroundColor = colorOfBlackFigures;

        switch (field % 10)
        {
            case 0: return ' ';
            case 1: return 'p';
            case 2: return 'N';
            case 3: return 'B';
            case 4: return 'R';
            case 5: return 'Q';
            case 6: return 'K';
            default: return ' ';
        }
    }

    public static char TakenFigure(int field)
    {
        if ((figures[0][0][0] - kingUnits < field) && (field <= figures[0][0][0]))
            Console.ForegroundColor = colorOfWhiteFigures;
        else
            Console.ForegroundColor = colorOfBlackFigures;

        switch (field % 10)
        {
            case 0: return ' ';
            case 1: return 'p';
            case 2: return 'N';
            case 3: return 'B';
            case 4: return 'R';
            case 5: return 'Q';
            case 6: return 'K';
            default: return ' ';
        }
    }

    public static void PrintLastMoves(bool makesClean = false)
    {
        if (makesClean)
            ClearMovesBlackboard();

        Console.BackgroundColor = colorOfBottomHelpBackground;
        Console.ForegroundColor = colorOfBottomHelpForeground;
        Console.SetCursorPosition(49, 0);
        Console.Write(topHelp);
        Console.SetCursorPosition(49, 31);
        Console.Write("Current mode: {0}   {1}", gameModes[(int)gameMode], bottomHelp);

        nextWhiteToMove = 0;
        Console.BackgroundColor = colorOfMovesBackground;
        Console.ForegroundColor = colorOfMovesForeground;
        if (0 < lastMoves[0].Count)
        {
            if (lastMoves[0].Count < 120)
            {
                for (i = 0; i < lastMoves[0].Count; i++)
                {
                    nextWhiteToMove = i + 1;
                    Console.SetCursorPosition(49 + (i / 30) * 26, 1 + (i % 30));
                    if (i < 90)
                        Console.Write("{0,2}. ", (i + 1));
                    else
                        Console.Write("{0,3}. ", (i + 1));

                    // promqnata ot sledva6tiq komentar e ottuk do nego

                    if (i < lastMoves[0].Count - 1)
                    {
                        Console.Write("{0, -11}", lastMoves[0][i]);
                        if (i < lastMoves[1].Count)
                            Console.Write("{0}", lastMoves[1][i]);
                    }
                    else
                    {
                        if ((realState == States.Free) || (realState == States.Check))
                        {
                            Console.Write("{0, -11}", lastMoves[0][i]);
                            if (i < lastMoves[1].Count)
                                Console.Write("{0}", lastMoves[1][i]);
                        }
                        else
                        {
                            if (lastMoves[1].Count < lastMoves[0].Count)
                            {
                                PreparingForPrintOfTheEndingMove();
                                Console.Write("{0, -11}", lastMoves[0][i]);
                            }
                            else
                            {
                                Console.Write("{0, -11}", lastMoves[0][i]);
                                PreparingForPrintOfTheEndingMove();
                                Console.Write("{0}", lastMoves[1][i]);
                            }
                        }
                    }

                    //Console.Write("{0, -11}", lastMoves[0][i]);
                    //if (i < lastMoves[1].Count)
                    //    Console.Write("{0}", lastMoves[1][i]);
                }

                if (gameMode != GameModes.CheckMoveList)
                    Console.CursorVisible = true;

                // dobaveno uslovie i za realState
                if ((gameMode != GameModes.CheckMoveList) && (lastMoves[0].Count == lastMoves[1].Count) && ((realState == States.Free) || (realState == States.Check)))
                {
                    Console.SetCursorPosition(49 + (nextWhiteToMove / 30) * 26, 1 + (nextWhiteToMove % 30));
                    if (nextWhiteToMove < 90)
                        Console.Write("{0,2}. ", nextWhiteToMove + 1);
                    else
                        Console.Write("{0,3}. ", nextWhiteToMove + 1);

                    Console.CursorVisible = true;
                }
            }
            else
            {
                mov = lastMoves[0].Count;
                colon = 0;
                for (i = mov - mov % 30 - 90; i < lastMoves[0].Count; i++)
                {
                    nextWhiteToMove = colon + 1;
                    Console.SetCursorPosition(49 + (colon / 30) * 26, 1 + (i % 30));
                    colon++;
                    if (i < 90)
                        Console.Write("{0,2}. ", (i + 1));
                    else
                        Console.Write("{0,3}. ", (i + 1));

                    if (i < lastMoves[0].Count - 1)
                    {
                        Console.Write("{0, -11}", lastMoves[0][i]);
                        if (i < lastMoves[1].Count)
                            Console.Write("{0}", lastMoves[1][i]);
                    }
                    else
                    {
                        if ((realState == States.Free) || (realState == States.Check))
                        {
                            Console.Write("{0, -11}", lastMoves[0][i]);
                            if (i < lastMoves[1].Count)
                                Console.Write("{0}", lastMoves[1][i]);
                        }
                        else
                        {
                            if (lastMoves[1].Count < lastMoves[0].Count)
                            {
                                PreparingForPrintOfTheEndingMove();
                                Console.Write("{0, -11}", lastMoves[0][i]);
                            }
                            else
                            {
                                Console.Write("{0, -11}", lastMoves[0][i]);
                                PreparingForPrintOfTheEndingMove();
                                Console.Write("{0}", lastMoves[1][i]);
                            }
                        }
                    }
                }

                if (gameMode != GameModes.CheckMoveList)
                    Console.CursorVisible = true;

                if ((gameMode != GameModes.CheckMoveList) && (lastMoves[0].Count == lastMoves[1].Count) && ((realState == States.Free) || (realState == States.Check)))
                {
                    Console.SetCursorPosition(49 + (nextWhiteToMove / 30) * 26, 1 + (nextWhiteToMove % 30));
                    if (nextWhiteToMove < 90)
                        Console.Write("{0,2}. ", nextWhiteToMove + 1);
                    else
                        Console.Write("{0,3}. ", nextWhiteToMove + 1);

                    Console.CursorVisible = true;
                }
            }
        }
        else
        {
            if (gameMode != GameModes.CheckMoveList)
            {
                Console.SetCursorPosition(49, 1);
                Console.Write("{0,2}. ", (1));
                Console.CursorVisible = true;
            }
        }

        if ((realState == States.Draw) || (realState == States.Checkmate) || (realState == States.Resign))
            Console.CursorVisible = false;
    }

    public static void ClearMovesBlackboard()
    {
        Console.CursorVisible = false;
        Console.BackgroundColor = colorOfMovesBackground;
        Console.ForegroundColor = colorOfMovesForeground;
        for (i = 0; i <= 31; i++)
        {
            Console.SetCursorPosition(48, i);
            Console.Write(emptyBlackboardRow);
        }
    }

    public static void PreparingForPrintOfTheEndingMove()
    {
        Console.CursorVisible = false;
        Console.BackgroundColor = colorOfTheEndingMoveBackground;
        Console.ForegroundColor = colorOfTheEndingMoveForeground;
    }

    #endregion

    #region Game Modes

    public static void Play()
    {
        //gameMode = GameModes.PlayerVsPlayer;
        //gameMode = GameModes.CheckMoveList;
        //gameMode = GameModes.PlayerVsComputer;
        //gameMode = GameModes.ComputerVsComputer;

        switch (gameMode)
        {
            case GameModes.PlayerVsComputer: { PlayerVsComputer(); break; }
            case GameModes.CheckMoveList: { CheckMoveList(); break; }
            case GameModes.PlayerVsPlayer: { PlayerVsPlayer(); break; }
            case GameModes.ComputerVsComputer: { ComputerVsComputer(); break; }
            default: break;
        }

        Ending();
    }

    public static void PlayerVsComputer()
    {
        StartingPosition();
        while ((realState != States.Draw) && (realState != States.Checkmate) && (realState != States.Resign))
        {
            NextPCMove();
            if (hadNewGame)
                return;

            if (realState != States.Resign)
                ChangePlayerToMove();
        }
    }

    public static void PlayerVsPlayer()
    {
        StartingPosition();
        while ((realState != States.Draw) && (realState != States.Checkmate) && (realState != States.Resign))
        {
            NextMove();
            if (hadNewGame)
                return;

            if (realState != States.Resign) // in original this condition is not necessary and table'll rotate at the end
                ChangePlayerToMove();
        }
    }

    public static void ComputerVsComputer()
    {
        StartingPosition();
        //sound = false;
        while ((realState != States.Draw) && (realState != States.Checkmate) && (realState != States.Resign))
        {
            NextOnlyPCsMove();
            if (hadNewGame)
                return;

            if (realState != States.Resign)
                ChangePlayerToMove();
        }
    }

    public static void CheckMoveList()
    {
        StartingPosition();
        if (!FillMoveList())
            return;

        //startTime = DateTime.Now;
        PrintBattlefield(lookingAngles[playerToMove]);
        while ((realState != States.Draw) && (realState != States.Checkmate) && (realState != States.Resign))
        {
            NextMoveFromList(moveList[currentMove]);
            if (hadNewGame)
                return;

            ChangePlayerToMove();
        }

        FullPrint();
        Console.CursorVisible = false;
        //Console.SetCursorPosition(50, 31);
        //Console.Write("{0}", DateTime.Now - startTime);
    }

    public static void Ending()
    {
        if (!hadNewGame)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(48, 0);
            Console.BackgroundColor = colorOfDefaultBackground;
            Console.ForegroundColor = colorOfDefaultForeground;
            Console.Write(emptyBlackboardRow);
            Console.SetCursorPosition(49, 0);
            Console.Write(playAgain);
            Console.CursorVisible = true;
            changeGameModeByEventually = Console.ReadLine();
            changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
            if (changeGameModeBy != -1)
                gameMode = (GameModes)changeGameModeBy;
        }
    }

    #endregion

    #region Players' moves

    public static void NextOnlyPCsMoveOldVersion()
    {
        FullPrint();
        Console.CursorVisible = true;
        //Thread.Sleep(timePCThinksMove);

        if (Console.KeyAvailable)
        {
            PrintLastMoves(true);
            pressed = Console.ReadKey().KeyChar.ToString();
            Console.CursorVisible = false;
            hadRotation = false;
            hadBlindfoldChange = false;

            #region Start new game immediately

            hadNewGame = false;
            changeGameModeByEventually = pressed;
            changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
            if (changeGameModeBy != -1)
            {
                gameMode = (GameModes)changeGameModeBy;
                hadNewGame = true;
            }

            #endregion

            #region Table rotation

            else if (rotations.IndexOf(pressed) != -1)
            {
                Rotate(oppositePlayer, pressed);
                hadRotation = true;
                ChangePlayerToMove();
                FullPrint();
                Console.CursorVisible = false;
            }

            #endregion

            #region Blindfold

            else if (pressed == changeBlindfold)
            {
                blindfold = !blindfold;
                hadBlindfoldChange = true;
                FullPrint();
                Console.CursorVisible = false;
                ChangePlayerToMove();
            }

            #endregion

            #region Sound

            else if (pressed == changeSound)
            {
                sound = !sound;
                PrintLastMoves(true);
                Console.CursorVisible = false;
                ChangePlayerToMove();
            }

            #endregion

            if (hadRotation || hadBlindfoldChange)
                Thread.Sleep(timeAfterRotationOrBlindfoldChangeBeforeNextMoveFromListOrPC);

            return;
        }

        FindAllPossibleMoves();
        ChoosePossibleMove();
        gotMove = buildTextOfMove.ToString();
        Console.Write(gotMove);
        Console.CursorVisible = false;

        AddCastleRestrictions();
        realState = give;

        //Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
        PrintBattlefield(lookingAngles[playerToMove]);
        if (sound)
            SayMove(MakeSentence());

        currentMove++;
        lastMoves[playerToMove].Add(gotMove);

        //if (gotMove == drawQuestion)
        //{
        //    PrintLastMoves();
        //    Console.Write(drawQuestion);
        //    if (Console.ReadLine() == drawAnswer)
        //    {
        //        currentMove++;
        //        gotMove = drawByAgreement;
        //        lastMoves[playerToMove].Add(gotMove);
        //        realState = give;
        //        SayMove();
        //    }
        //}
    }

    public static void NextOnlyPCsMove()
    {
        FullPrint();
        Console.CursorVisible = true;
        Thread.Sleep(timePCThinksMove);
        //Thread.Sleep(50);
        if (Console.KeyAvailable)
        {
            PrintLastMoves(true);
            pressed = Console.ReadKey().KeyChar.ToString();
            Console.CursorVisible = false;
            hadRotation = false;
            hadBlindfoldChange = false;

            #region Start new game immediately

            hadNewGame = false;
            changeGameModeByEventually = pressed;
            changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
            if (changeGameModeBy != -1)
            {
                gameMode = (GameModes)changeGameModeBy;
                hadNewGame = true;
            }

            #endregion

            #region Table rotation

            else if (rotations.IndexOf(pressed) != -1)
            {
                Rotate(oppositePlayer, pressed);
                hadRotation = true;
                ChangePlayerToMove();
                FullPrint();
                Console.CursorVisible = false;
            }

            #endregion

            #region Blindfold

            else if (pressed == changeBlindfold)
            {
                blindfold = !blindfold;
                hadBlindfoldChange = true;
                FullPrint();
                Console.CursorVisible = false;
                ChangePlayerToMove();
            }

            #endregion

            #region Sound

            else if (pressed == changeSound)
            {
                sound = !sound;
                PrintLastMoves(true);
                Console.CursorVisible = false;
                ChangePlayerToMove();
            }

            #endregion

            if (hadRotation || hadBlindfoldChange)
                Thread.Sleep(timeAfterRotationOrBlindfoldChangeBeforeNextMoveFromListOrPC);

            return;
        }

        if (gotMove == drawQuestion)
        {
            FullPrint();
            Console.Write(drawQuestion);
            Console.CursorVisible = true;
            Thread.Sleep(timePCThinksMove);
            if (randomGenerator.Next(1, randomAcceptDrawByAgreementFrom + 1) <= randomAcceptDrawByAgreement)
            {
                currentMove++;
                gotMove = drawByAgreement;
                lastMoves[playerToMove].Add(gotMove);
                give = States.Draw;
                realState = give;
                FullPrint();
                if (sound)
                    SayMove(MakeSentence());

                return;
            }
            else
            {
                Console.Write(drawAnswerRefuse);
                Console.CursorVisible = false;
                if (sound)
                    SayMove(string.Format("{0}{1}", playWith == 0 ? "Black" : "White", " refuse draw by agreement!"));
            }

            return;
        }

        if (randomGenerator.Next(1, randomResignFrom + 1) <= randomResign)
        {
            Console.Write(gotMove);
            Console.CursorVisible = false;
            gotMove = resign;
            currentMove++;
            lastMoves[playerToMove].Add(gotMove);
            give = States.Resign;
            realState = give;
            FullPrint();
            if (sound)
                SayMove(MakeSentence());

            return;
        }

        if (randomGenerator.Next(1, randomOffersDrawByAgreementFrom + 1) <= randomOffersDrawByAgreement)
        {
            Console.Write(drawQuestion);
            Console.CursorVisible = false;
            if (sound)
                SayMove(string.Format("{0}{1}", playWith == 0 ? "Black" : "White", " offers draw by agreement!"));

            Console.CursorVisible = true;
            if (randomGenerator.Next(1, randomAcceptDrawByAgreementFrom + 1) <= randomAcceptDrawByAgreement)
            {
                Console.CursorVisible = false;
                gotMove = drawByAgreement;
                currentMove++;
                lastMoves[playerToMove].Add(gotMove);
                give = States.Draw;
                realState = give;
                FullPrint();
                if (sound)
                    SayMove(string.Format("{0}{1}", playWith == 0 ? "White" : "Black", "  accepts draw by agreement!"));

                return;
            }
            else
                ChangePlayerToMove();

            return;
        }

        FindAllPossibleMoves();
        ChoosePossibleMove();
        gotMove = buildTextOfMove.ToString();
        //Console.Write(gotMove);
        Console.CursorVisible = false;
        realState = give;
        if ((realState == States.Draw) || (realState == States.Resign))
        {
            currentMove++;
            lastMoves[playerToMove].Add(gotMove);
            FullPrint();
            if (sound)
                SayMove(MakeSentence());

            return;
        }

        Console.Write(gotMove);
        Console.CursorVisible = false;
        Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
        PrintBattlefield(lookingAngles[playerToMove]);
        if (sound)
            SayMove(MakeSentence());

        AddCastleRestrictions();
        currentMove++;
        lastMoves[playerToMove].Add(gotMove);
    }

    public static void NextPCMove()
    {
        #region Player's move

        if (playerToMove == playWith)
        {
            do
            {
                if ((0 < currentMove)
                && ((gotMove != changeBlindfold) && (lookingAngles[0] == lookingAngles[1]))
                        || (gotMove == changeSound))
                    PrintLastMoves(makesClean: true);
                else
                    FullPrint();

                gotMove = Console.ReadLine();
                Console.CursorVisible = false;
                if (currentMove == 0)
                {
                    if (gotMove == changePlayWith)
                    {
                        oppositePlayWith = playWith;
                        playWith = 1 - playWith;
                        lookingAngles[playerToMove] = lookingAngles[oppositePlayer] = 2 * playWith;
                        FullPrint();
                        Console.Write(gotMove);
                        Console.CursorVisible = false;
                        ChangePlayerToMove();
                        break;
                    }
                    else
                    {
                        FullPrint();
                        Console.Write(gotMove);
                        Console.CursorVisible = false;
                    }
                }

                #region Start new game immediately

                hadNewGame = false;
                changeGameModeByEventually = gotMove;
                changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
                if (changeGameModeBy != -1)
                {
                    gameMode = (GameModes)changeGameModeBy;
                    hadNewGame = true;
                    return;
                }

                #endregion

                #region Table rotation

                if (rotations.IndexOf(gotMove) != -1)
                {
                    Rotate(playerToMove, gotMove);
                    PrintBattlefield(lookingAngles[playerToMove]);
                    ChangePlayerToMove();
                    return;
                }

                #endregion

                #region Blindfold

                if (gotMove == changeBlindfold)
                {
                    blindfold = !blindfold;
                    ChangePlayerToMove();
                    return;
                }

                #endregion

                #region Sound

                if (gotMove == changeSound)
                {
                    sound = !sound;
                    ChangePlayerToMove();
                    return;
                }

                #endregion

                if (ReadMove() && IsLegalMove())
                {
                    if ((realState == States.Draw) || (realState == States.Resign))
                    {
                        currentMove++;
                        lastMoves[playerToMove].Add(gotMove);
                        FullPrint();
                        if (sound)
                            SayMove(MakeSentence());

                        return;
                    }

                    Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
                    PrintBattlefield(lookingAngles[playerToMove]);
                    if (sound)
                        SayMove(MakeSentence());

                    if (gotMove == drawQuestion)
                    {
                        FullPrint();
                        Console.Write(drawQuestion);
                        Console.CursorVisible = true;
                        Thread.Sleep(timePCThinksMove);
                        if (randomGenerator.Next(1, randomAcceptDrawByAgreementFrom + 1) <= randomAcceptDrawByAgreement)
                        {
                            currentMove++;
                            gotMove = drawByAgreement;
                            lastMoves[playerToMove].Add(gotMove);
                            give = States.Draw;
                            realState = give;
                            FullPrint();
                            if (sound)
                                SayMove(MakeSentence());

                            break;
                        }
                        else
                        {
                            Console.Write(drawAnswerRefuse);
                            Console.CursorVisible = false;
                            if (sound)
                                SayMove(string.Format("{0}{1}", playWith == 0 ? "Black" : "White", " refuse draw by agreement!"));
                        }

                        continue;
                    }

                    AddCastleRestrictions();
                    currentMove++;
                    lastMoves[playerToMove].Add(gotMove);
                    realState = give;
                    break;
                }
            } while (true);
        }

        #endregion

        #region PC's move

        else
        {
            FullPrint();
            Console.CursorVisible = true;
            Thread.Sleep(timePCThinksMove);
            if (randomGenerator.Next(1, randomResignFrom + 1) <= randomResign)
            {
                Console.Write(gotMove);
                Console.CursorVisible = false;
                gotMove = resign;
                currentMove++;
                lastMoves[playerToMove].Add(gotMove);
                give = States.Resign;
                realState = give;
                FullPrint();
                if (sound)
                    SayMove(MakeSentence());

                return;
            }

            if (randomGenerator.Next(1, randomOffersDrawByAgreementFrom + 1) <= randomOffersDrawByAgreement)
            {
                Console.Write(drawQuestion);
                Console.CursorVisible = false;
                if (sound)
                    SayMove(string.Format("{0}{1}", playWith == 0 ? "Black" : "White", " offers draw by agreement!"));

                Console.CursorVisible = true;
                if (Console.ReadLine() == drawAnswer)
                {
                    Console.CursorVisible = false;
                    gotMove = drawByAgreement;
                    currentMove++;
                    lastMoves[playerToMove].Add(gotMove);
                    give = States.Draw;
                    realState = give;
                    FullPrint();
                    if (sound)
                        SayMove(string.Format("{0}{1}", playWith == 0 ? "White" : "Black", "  accepts draw by agreement!"));

                    return;
                }
                else
                    ChangePlayerToMove();

                return;
            }

            FindAllPossibleMoves();
            ChoosePossibleMove();
            gotMove = buildTextOfMove.ToString();
            Console.Write(gotMove);
            Console.CursorVisible = false;
            Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
            PrintBattlefield(lookingAngles[playerToMove]);
            if (sound)
                SayMove(MakeSentence());

            if (gotMove == drawQuestion)
            {
                FullPrint();
                Console.Write(drawQuestion);
                Console.CursorVisible = true;
                Thread.Sleep(timePCThinksMove);
                if (Console.ReadLine() == drawAnswer)
                {
                    currentMove++;
                    gotMove = drawByAgreement;
                    lastMoves[playerToMove].Add(gotMove);
                    realState = give;
                    if (sound)
                        SayMove(MakeSentence());
                }
            }

            AddCastleRestrictions();
            currentMove++;
            lastMoves[playerToMove].Add(gotMove);
            realState = give;
        }

        #endregion
    }

    public static void NextMoveFromList(string giveMove)
    {
        do
        {
            if (currentMove == 0)
                Thread.Sleep(timeBeforeFirstMoveFromList);

            if (Console.KeyAvailable)
            {
                PrintLastMoves(true);
                pressed = Console.ReadKey().KeyChar.ToString();
                Console.CursorVisible = false;
                hadRotation = false;
                hadBlindfoldChange = false;

                #region Start new game immediately

                hadNewGame = false;
                changeGameModeByEventually = pressed;
                changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
                if (changeGameModeBy != -1)
                {
                    gameMode = (GameModes)changeGameModeBy;
                    hadNewGame = true;
                }

                #endregion

                #region Table rotation

                else if (rotations.IndexOf(pressed) != -1)
                {
                    Rotate(oppositePlayer, pressed);
                    hadRotation = true;
                    ChangePlayerToMove();
                    FullPrint();
                    Console.CursorVisible = false;
                }

                #endregion

                #region Blindfold

                else if (pressed == changeBlindfold)
                {
                    blindfold = !blindfold;
                    hadBlindfoldChange = true;
                    FullPrint();
                    Console.CursorVisible = false;
                    ChangePlayerToMove();
                }

                #endregion

                #region Sound

                else if (pressed == changeSound)
                {
                    sound = !sound;
                    PrintLastMoves(true);
                    Console.CursorVisible = false;
                    ChangePlayerToMove();
                }

                #endregion

                if (hadRotation || hadBlindfoldChange)
                    Thread.Sleep(timeAfterRotationOrBlindfoldChangeBeforeNextMoveFromListOrPC);

                return;
            }
            else
            {
                if (currentMove == 0)
                    PrintBattlefield(lookingAngles[playerToMove]);

                gotMove = giveMove;
                if (ReadMove() && IsLegalMove())
                {
                    AddCastleRestrictions();
                    currentMove++;
                    lastMoves[playerToMove].Add(gotMove);
                    realState = give;
                    ///////////////////////FullPrint();
                    PrintLastMoves(true);
                    Console.CursorVisible = false;
                    Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
                    PrintBattlefield(lookingAngles[playerToMove]);
                    if (sound)
                        SayMove(MakeSentence());

                    Thread.Sleep(timeBeforeNextMoveFromList);
                    break;
                }
            }
        } while (true);
    }

    public static void NextMove()
    {
        do
        {
            if ((0 < currentMove)
                && ((gotMove != changeBlindfold) && (lookingAngles[0] == lookingAngles[1]))
                        || (gotMove == changeSound))
                PrintLastMoves(makesClean: true);
            else
                FullPrint();

            gotMove = Console.ReadLine();
            Console.CursorVisible = false;

            #region Start new game immediately

            hadNewGame = false;
            changeGameModeByEventually = gotMove;
            changeGameModeBy = gameModes.IndexOf(changeGameModeByEventually);
            if (changeGameModeBy != -1)
            {
                gameMode = (GameModes)changeGameModeBy;
                hadNewGame = true;
                return;
            }

            #endregion

            #region Table rotation

            if (rotations.IndexOf(gotMove) != -1)
            {
                Rotate(playerToMove, gotMove);
                PrintBattlefield(lookingAngles[playerToMove]);
                ChangePlayerToMove();
                return;
            }

            #endregion

            #region Blindfold

            if (gotMove == changeBlindfold)
            {
                blindfold = !blindfold;
                ChangePlayerToMove();
                return;
            }

            #endregion

            #region Sound

            if (gotMove == changeSound)
            {
                sound = !sound;
                ChangePlayerToMove();
                return;
            }

            #endregion

            if (ReadMove() && IsLegalMove())
            {
                if ((realState == States.Draw) || (realState == States.Resign))
                {
                    currentMove++;
                    lastMoves[playerToMove].Add(gotMove);
                    FullPrint();
                    if (sound)
                        SayMove(MakeSentence());

                    return;
                }

                Thread.Sleep(timeBetweenPrintMoveAndMoveFigure);
                PrintBattlefield(lookingAngles[playerToMove]);
                if (sound)
                    SayMove(MakeSentence());

                if (gotMove == drawQuestion)
                {
                    FullPrint();
                    Console.Write(drawQuestion);
                    if (Console.ReadLine() == drawAnswer)
                    {
                        currentMove++;
                        gotMove = drawByAgreement;
                        lastMoves[playerToMove].Add(gotMove);
                        give = States.Draw;
                        realState = give;
                        FullPrint();
                        if (sound)
                            SayMove(MakeSentence());

                        break;
                    }

                    continue;
                }

                AddCastleRestrictions();
                currentMove++;
                lastMoves[playerToMove].Add(gotMove);
                realState = give;
                break;
            }
        } while (true);
    }

    public static bool ReadMove()
    {
        takes = false;
        exchange = false;

        #region Resign

        if (gotMove == resign)
        {
            give = States.Resign;
            realState = give;
            return true;
        }

        #endregion

        #region Draw

        #region Draw by agreement

        if ((gotMove == drawQuestion)
                && ((gameMode == GameModes.PlayerVsPlayer) || (gameMode == GameModes.PlayerVsComputer) || (gameMode == GameModes.ComputerVsComputer)))
        {
            give = States.Draw;
            return true;
        }

        if ((gotMove == drawByAgreement) && (gameMode == GameModes.CheckMoveList))
        {
            give = States.Draw;
            return true;
        }

        #endregion

        #region Draw by insufficient material, by repetition and by fifty-rule

        if (gotMove == draw)
        {
            if ((((materials[0] == whiteKing) || (materials[0] == whiteKingPlusNight) || (materials[0] == whiteKingPlusBishop))
                    && ((materials[1] == blackKing) || (materials[1] == blackKingPlusNight) || (materials[1] == blackKingPlusBishop)))
                || ((3 <= maxMatchesEventually)
                || (50 <= saveBattlefields.Count)))
            {
                give = States.Draw;
                return true;
            }
            else
                return false;
        }

        #endregion

        #endregion

        #region Castle

        castle = Castles.noCastle;
        if (gotMove.IndexOf(someCastle) != -1)
        {
            if (gotMove == castleKingsideFree)
            {
                give = States.Free;
                castle = Castles.castleKingsideFree;
                return castleKingside = true;
            }
            else if (gotMove == castleKingsideCheck)
            {
                give = States.Check;
                castle = Castles.castleKingsideCheck;
                return castleKingside = true;
            }
            else if (gotMove == castleKingsideDraw)
            {
                give = States.Draw;
                castle = Castles.castleKingsideDraw;
                return castleKingside = true;
            }
            else if (gotMove == castleKingsideCheckmate)
            {
                give = States.Checkmate;
                castle = Castles.castleKingsideCheckmate;
                return castleKingside = true;
            }
            else if (gotMove == castleQueensideFree)
            {
                give = States.Free;
                castle = Castles.castleQueensideFree;
                return castleQueenside = true;
            }
            else if (gotMove == castleQueensideCheck)
            {
                give = States.Check;
                castle = Castles.castleQueensideCheck;
                return castleQueenside = true;
            }
            else if (gotMove == castleQueensideDraw)
            {
                give = States.Draw;
                castle = Castles.castleQueensideDraw;
                return castleQueenside = true;
            }
            else if (gotMove == castleQueensideCheckmate)
            {
                give = States.Checkmate;
                castle = Castles.castleQueensideCheckmate;
                return castleQueenside = true;
            }
            else
                return false;
        }

        #endregion

        #region General inspection by length of gotMove

        if ((gotMove.Length < 5) || (9 < gotMove.Length))
            return false;

        #endregion

        #region Creating of current queue

        readMove.Clear();
        for (i = 0; i < gotMove.Length; i++)
            readMove.Enqueue(gotMove[i]);

        #endregion

        #region Player's to move catch the figure

        savePositions[playerToMove][0][0] = specialFigures.IndexOf(readMove.Peek());
        if (savePositions[playerToMove][0][0] != -1)
        {
            if (playerToMove == 0)
                savePositions[playerToMove][0][0] += whiteDecimals + nightUnits;
            else
                savePositions[playerToMove][0][0] += blackDecimals + nightUnits;

            readMove.Dequeue();
            if (readMove.Count < 5)
                return false;
        }
        else
        {
            if (playerToMove == 0)
                savePositions[playerToMove][0][0] = whiteDecimals + pawnUnits;
            else
                savePositions[playerToMove][0][0] = blackDecimals + pawnUnits;
        }

        #endregion

        #region from field with column

        savePositions[playerToMove][0][2] = allColumns.IndexOf(readMove.Peek());
        if (savePositions[playerToMove][0][2] == -1)
            return false;

        readMove.Dequeue();

        #endregion

        #region and row

        savePositions[playerToMove][0][1] = (int)readMove.Peek() - 49;
        if ((savePositions[playerToMove][0][1] < 0) || (7 < savePositions[playerToMove][0][1]))
            return false;

        readMove.Dequeue();

        #endregion

        #region and moves it to (and takes the figure on)

        if ((readMove.Peek() == 'x') || (readMove.Peek() == ':'))
            takes = true;
        else if (readMove.Peek() != '-')
            return false;

        readMove.Dequeue();

        #endregion

        #region field with column

        savePositions[playerToMove][1][2] = allColumns.IndexOf(readMove.Peek());
        if (savePositions[playerToMove][1][2] == -1)
            return false;

        readMove.Dequeue();

        #endregion

        #region and row

        savePositions[playerToMove][1][1] = (int)readMove.Peek() - 49;
        if ((savePositions[playerToMove][1][1] < 0) || (7 < savePositions[playerToMove][1][1]))
            return false;

        savePositions[playerToMove][1][0] = battlefield[savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]];

        #endregion

        #region (exchange pawn with...) and give...

        readMove.Dequeue();
        if (readMove.Count == 0)
        {
            give = States.Free;
            return true;
        }
        else if (readMove.Count == 1)
        {
            if (readMove.Peek() == '#')
                give = States.Checkmate;
            else if (readMove.Peek() == '+')
                give = States.Check;
            else if (readMove.Peek() == '=')
                give = States.Draw;
            else
                return false;

            return true;
        }
        else if ((readMove.Count == 2) || (readMove.Count == 3))
        {
            if (readMove.Peek() != '=')
                return false;

            readMove.Dequeue();
            exchangeWith = specialFigures.IndexOf(readMove.Peek());
            if (exchangeWith == -1)
                return false;

            exchange = true;
            exchangeWith += figures[playerToMove][0][0] - kingUnits + nightUnits;
            if (readMove.Count == 1)
                return true;

            readMove.Dequeue();
            if (readMove.Peek() == '#')
                give = States.Checkmate;
            else if (readMove.Peek() == '+')
                give = States.Check;
            else if (readMove.Peek() == '=')
                give = States.Draw;
            else
                return false;

            return true;
        }
        else
            return false;

        #endregion
    }

    public static bool Rotate(int player, string lastGotMove)
    {
        for (rotate = 0; rotate < rotations.Count; rotate++)
            if (lastGotMove == rotations[rotate])
            {
                lookingAngle = lookingAngles[player];
                switch (rotate)
                {
                    case 0: { lookingAngle = 2 * player; break; }
                    case 1: { lookingAngle++; break; }
                    case 2: { lookingAngle = 2 * player + 2; break; }
                    case 3: { lookingAngle += 3; break; }
                    default: break;
                }

                if (3 < lookingAngle)
                    lookingAngle -= 4;

                lookingAngles[player] = lookingAngle;
                if ((gameMode == GameModes.PlayerVsComputer) || (gameMode == GameModes.ComputerVsComputer) || (gameMode == GameModes.CheckMoveList))
                    lookingAngles[1 - player] = lookingAngle;

                return true;
            }

        return false;
    }

    public static bool IsLegalMove()
    {
        if (give == States.Resign)
            return true;

        if (((gameMode == GameModes.PlayerVsPlayer) || (gameMode == GameModes.PlayerVsComputer) || (gameMode == GameModes.ComputerVsComputer))
                && (gotMove == drawQuestion))
            return true;

        if ((gameMode == GameModes.CheckMoveList) && (gotMove == drawByAgreement))
            return true;

        if ((gotMove == draw)
                && ((materials[0] == whiteKing) || (materials[0] == whiteKingPlusNight) || (materials[0] == whiteKingPlusBishop))
                    && ((materials[1] == blackKing) || (materials[1] == blackKingPlusNight) || (materials[1] == blackKingPlusBishop)))
        {
            give = States.Draw;
            realState = give;
            return true;
        }

        if (castle != Castles.noCastle)
        {
            if (!CastlePossibility())
                return false;

            makeNextCheck = true;
        }
        else
        {
            if (((savePositions[playerToMove][0][1] == savePositions[playerToMove][1][1]) && (savePositions[playerToMove][0][2] == savePositions[playerToMove][1][2]))
                    || (battlefield[savePositions[playerToMove][0][1], savePositions[playerToMove][0][2]] != savePositions[playerToMove][0][0])
                        || (!takes && (battlefield[savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]] != 0)))
                return false;

            if (takes
                    && ((battlefield[savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]] <= figures[oppositePlayer][0][0] - kingUnits)
                        || (figures[oppositePlayer][0][0] <= battlefield[savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]])))
                return false;

            canJump = true;
            switch (savePositions[playerToMove][0][0] % 10)
            {
                case kingUnits: { canJump = JumpOfKing(playerToMove, savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]); break; }
                case queenUnits: { canJump = JumpOfQueen(savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]); break; }
                case rookUnits: { canJump = JumpOfRook(savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]); break; }
                case bishopUnits: { canJump = JumpOfBishop(savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]); break; }
                case nightUnits: { canJump = JumpOfNight(savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]); break; }
                case pawnUnits: { canJump = JumpOfPawn(playerToMove, savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2], takes, exchange); break; }
                default: break;
            }

            if (!canJump)
                return false;

            savePositions[playerToMove][0][3] = ReturnIndexOfFigure(playerToMove, savePositions[playerToMove][0][1], savePositions[playerToMove][0][2]);
            figures[playerToMove][1][savePositions[playerToMove][0][3]] = savePositions[playerToMove][1][1];
            figures[playerToMove][2][savePositions[playerToMove][0][3]] = savePositions[playerToMove][1][2];
            if (takes)
            {
                savePositions[playerToMove][1][3] = ReturnIndexOfFigure(oppositePlayer, savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]);
                materials[oppositePlayer] -= figures[oppositePlayer][0][savePositions[playerToMove][1][3]];
                takenFigures[oppositePlayer].Add(figures[oppositePlayer][0][savePositions[playerToMove][1][3]]);
                RemoveFigureAtPosition(oppositePlayer, savePositions[playerToMove][1][3]);
            }

            if (exchange)
            {
                materials[playerToMove] += exchangeWith - figures[playerToMove][0][savePositions[playerToMove][0][3]];
                figures[playerToMove][0][savePositions[playerToMove][0][3]] = exchangeWith;
                MakeMove(0, savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], exchangeWith, savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]);
            }
            else
                MakeMove(0, savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][0][0], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]);

            makeNextCheck = !Checks(playerToMove);
        }

        if (makeNextCheck)
        {
            if ((give == States.Draw)
                    && ((materials[0] == whiteKing) || (materials[0] == whiteKingPlusNight) || (materials[0] == whiteKingPlusBishop))
                        && ((materials[1] == blackKing) || (materials[1] == blackKingPlusNight) || (materials[1] == blackKingPlusBishop)))
            {
                realState = give;
                return true;
            }

            oppositeCheck = Checks(oppositePlayer);
            if ((((give == States.Free) || (give == States.Draw)) && oppositeCheck)
                    || (((give == States.Check) || (give == States.Checkmate)) && !oppositeCheck))
                makeNextCheck = false;
            else
            {
                oppositeAvoid = OppositeAvoid();
                if (((give == States.Checkmate) && oppositeAvoid)
                        || (((give == States.Free) || (give == States.Check)) && !oppositeAvoid))
                    makeNextCheck = false;

                if ((give == States.Draw) && oppositeAvoid)
                {
                    if ((savePositions[playerToMove][0][0] % 10 == pawnUnits) || takes)
                        makeNextCheck = false;
                    else
                    {
                        SaveCurrentBattlefield(false);
                        if ((3 <= maxMatchesEventually) || (50 <= saveBattlefields.Count))
                        {
                            maxMatches = maxMatchesEventually;
                            realState = give;
                            return true;
                        }
                        else
                        {
                            saveBattlefields.RemoveAt(saveBattlefields.Count - 1);
                            matches.RemoveAt(matches.Count - 1);
                            makeNextCheck = false;
                        }
                    }
                }
            }
        }

        if (makeNextCheck)
        {
            if ((savePositions[playerToMove][0][0] % 10 == pawnUnits) || takes)
                ClearAllSavedBattlefields();
            else
                SaveCurrentBattlefield(true);
        }

        if (!makeNextCheck)
        {
            if (castle != Castles.noCastle)
            {
                figures[playerToMove][2][0] = 4;
                if ((castle == Castles.castleKingsideFree) || (castle == Castles.castleKingsideCheck) || (castle == Castles.castleKingsideDraw) || (castle == Castles.castleKingsideCheckmate))
                {
                    MakeMove(0, castleRow, 6, king, castleRow, 4);
                    MakeMove(0, castleRow, 5, king - kingUnits + rookUnits, castleRow, 7);
                    figures[playerToMove][2][indexOfCastleRook] = 7;
                }
                else
                {
                    MakeMove(0, castleRow, 2, king, castleRow, 4);
                    MakeMove(0, castleRow, 3, king - kingUnits + rookUnits, castleRow, 0);
                    figures[playerToMove][2][indexOfCastleRook] = 0;
                }
            }
            else
            {
                figures[playerToMove][0][savePositions[playerToMove][0][3]] = savePositions[playerToMove][0][0];
                figures[playerToMove][1][savePositions[playerToMove][0][3]] = savePositions[playerToMove][0][1];
                figures[playerToMove][2][savePositions[playerToMove][0][3]] = savePositions[playerToMove][0][2];
                MakeMove(savePositions[playerToMove][0][0], savePositions[playerToMove][0][1], savePositions[playerToMove][0][2], savePositions[playerToMove][1][0], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]);
                if (takes)
                {
                    AddFigureAtPosition(oppositePlayer, savePositions[playerToMove][1][3], savePositions[playerToMove][1][0], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2]);
                    materials[oppositePlayer] += figures[oppositePlayer][0][savePositions[playerToMove][1][3]];
                    takenFigures[oppositePlayer].RemoveAt(takenFigures[oppositePlayer].Count - 1);
                }

                if (exchange)
                    materials[playerToMove] -= exchangeWith - figures[playerToMove][0][savePositions[playerToMove][0][3]];
            }
        }

        return makeNextCheck;
    }

    public static void ClearAllSavedBattlefields()
    {
        saveBattlefields.Clear();
        matches.Clear();
        maxMatches = 0;
    }

    public static void SaveCurrentBattlefield(bool rightMove)
    {
        saveCurrentBattlefield.Clear();
        for (i = 0; i <= 7; i++)
            for (j = 0; j <= 7; j++)
                saveCurrentBattlefield.Append(battlefield[i, j]);

        saveBattlefields.Add(saveCurrentBattlefield.ToString());
        matches.Add(0);
        indexOfCurrentBattlefield = saveBattlefields.Count - 1;
        for (k = 0; k <= indexOfCurrentBattlefield; k++)
            if (saveBattlefields[indexOfCurrentBattlefield] == saveBattlefields[k])
                matches[k]++;

        maxMatchesEventually = maxMatches;
        for (k = 0; k < matches.Count; k++)
            if (rightMove)
            {
                if (maxMatches < matches[k])
                    maxMatches = matches[k];
            }
            else
            {
                if (maxMatchesEventually < matches[k])
                    maxMatchesEventually = matches[k];
            }
    }

        #region Castle

    public static bool CastlePossibility()
    {
        if (realState != States.Free)
            return false;

        king = figures[playerToMove][0][0];
        if (playerToMove == 0)
            castleRow = 0;
        else
            castleRow = 7;

        if ((castle == Castles.castleKingsideFree) || (castle == Castles.castleKingsideCheck) || (castle == Castles.castleKingsideDraw) || (castle == Castles.castleKingsideCheckmate))
        {
            if (!(castlePossibility[playerToMove][0] && castlePossibility[playerToMove][1]))
                return false;

            if ((battlefield[castleRow, 5] != 0) || (battlefield[castleRow, 6] != 0))
                return false;

            MakeMove(0, castleRow, 4, king, castleRow, 5);
            figures[playerToMove][2][0] = 5;
            if (Checks(playerToMove))
            {
                MakeMove(0, castleRow, 5, king, castleRow, 4);
                figures[playerToMove][2][0] = 4;
                return false;
            }

            MakeMove(0, castleRow, 5, king, castleRow, 6);
            figures[playerToMove][2][0] = 6;
            if (Checks(playerToMove))
            {
                MakeMove(0, castleRow, 6, king, castleRow, 4);
                figures[playerToMove][2][0] = 4;
                return false;
            }

            MakeMove(0, castleRow, 7, king - kingUnits + rookUnits, castleRow, 5);
            indexOfCastleRook = ReturnIndexOfFigure(playerToMove, castleRow, 7);
            figures[playerToMove][2][indexOfCastleRook] = 5;
        }
        else
        {
            if (!(castlePossibility[playerToMove][0] && castlePossibility[playerToMove][2]))
                return false;

            if ((battlefield[castleRow, 3] != 0) || (battlefield[castleRow, 2] != 0) || (battlefield[castleRow, 1] != 0))
                return false;

            MakeMove(0, castleRow, 4, king, castleRow, 3);
            figures[playerToMove][2][0] = 3;
            if (Checks(playerToMove))
            {
                MakeMove(0, castleRow, 3, king, castleRow, 4);
                figures[playerToMove][2][0] = 4;
                return false;
            }

            MakeMove(0, castleRow, 3, king, castleRow, 2);
            figures[playerToMove][2][0] = 2;
            if (Checks(playerToMove))
            {
                MakeMove(0, castleRow, 2, king, castleRow, 4);
                figures[playerToMove][2][0] = 4;
                return false;
            }

            MakeMove(0, castleRow, 0, king - kingUnits + rookUnits, castleRow, 3);
            indexOfCastleRook = ReturnIndexOfFigure(playerToMove, castleRow, 0);
            figures[playerToMove][2][indexOfCastleRook] = 3;
        }

        return true;
    }

    public static void AddCastleRestrictions()
    {
        if (battlefield[0, 4] != whiteKing)
            castlePossibility[0][0] = false;
        else if (battlefield[0, 7] != whiteRook)
            castlePossibility[0][1] = false;
        else if (battlefield[0, 0] != whiteRook)
            castlePossibility[0][2] = false;

        if (battlefield[7, 4] != blackKing)
            castlePossibility[1][0] = false;
        else if (battlefield[7, 7] != blackRook)
            castlePossibility[1][1] = false;
        else if (battlefield[7, 0] != blackRook)
            castlePossibility[1][2] = false;
    }

        #endregion

    public static int ReturnIndexOfFigure(int player, int onRow, int onColumn)
    {
        for (i = 0; i < figures[player][0].Count; i++)
            if ((figures[player][1][i] == onRow) && (figures[player][2][i] == onColumn))
                return i;

        return -1;
    }

        #region Jumps

    public static bool JumpOfKing(int player, int fromRow, int fromColumn, int toRow, int toColumn)
    {
        if ((-1 <= toRow - fromRow) && (toRow - fromRow <= 1) && (-1 <= toColumn - fromColumn) && (toColumn - fromColumn <= 1)
                && ((toRow - figures[1 - player][1][0] < -1) || (1 < toRow - figures[1 - player][1][0]) || (toColumn - figures[1 - player][2][0] < -1) || (1 < toColumn - figures[1 - player][2][0])))
            return true;
        else
            return false;
    }

    public static bool JumpOfQueen(int fromRow, int fromColumn, int toRow, int toColumn)
    {
        return (JumpOfRook(fromRow, fromColumn, toRow, toColumn) || JumpOfBishop(fromRow, fromColumn, toRow, toColumn));
    }

    public static bool JumpOfRook(int fromRow, int fromColumn, int toRow, int toColumn)
    {
        if (toRow == fromRow)
        {
            if (toColumn < fromColumn)
            {
                for (j = toColumn + 1; j < fromColumn; j++)
                    if (battlefield[toRow, j] != 0)
                        return false;
            }
            else
                for (j = fromColumn + 1; j < toColumn; j++)
                    if (battlefield[toRow, j] != 0)
                        return false;
        }
        else if (toColumn == fromColumn)
        {
            if (toRow < fromRow)
            {
                for (i = toRow + 1; i < fromRow; i++)
                    if (battlefield[i, toColumn] != 0)
                        return false;
            }
            else
                for (i = fromRow + 1; i < toRow; i++)
                    if (battlefield[i, toColumn] != 0)
                        return false;
        }

        return true;
    }

    public static bool JumpOfBishop(int fromRow, int fromColumn, int toRow, int toColumn)
    {
        if ((toRow == fromRow) || (toColumn == fromColumn))
            return false;

        if ((int)Math.Abs(toRow - fromRow) != (int)Math.Abs(toColumn - fromColumn))
            return false;

        columnChange = 1;
        if (toRow < fromRow)
        {
            if (toColumn > fromColumn)
                columnChange = -1;

            for (i = toRow + 1; i < fromRow; i++)
                if (battlefield[i, toColumn + (i - toRow) * columnChange] != 0)
                    return false;
        }
        else
        {
            if (toColumn < fromColumn)
                columnChange = -1;

            for (i = fromRow + 1; i < toRow; i++)
                if (battlefield[i, fromColumn + (i - fromRow) * columnChange] != 0)
                    return false;
        }

        return true;
    }

    public static bool JumpOfNight(int fromRow, int fromColumn, int toRow, int toColumn)
    {
        if (((toRow == fromRow - 2) && (toColumn == fromColumn - 1))
                || ((toRow == fromRow - 2) && (toColumn == fromColumn + 1))
                || ((toRow == fromRow - 1) && (toColumn == fromColumn - 2))
                || ((toRow == fromRow - 1) && (toColumn == fromColumn + 2))
                || ((toRow == fromRow + 1) && (toColumn == fromColumn - 2))
                || ((toRow == fromRow + 1) && (toColumn == fromColumn + 2))
                || ((toRow == fromRow + 2) && (toColumn == fromColumn - 1))
                || ((toRow == fromRow + 2) && (toColumn == fromColumn + 1)))
            return true;
        else
            return false;
    }

    public static bool JumpOfPawn(int player, int fromR, int fromColumn, int toRow, int toColumn, bool taking = false, bool exchanging = false)
    {
        if ((toRow == fromR) || (toColumn - fromColumn < -1) || (1 < toColumn - fromColumn) || (toRow - fromR < -2) || (2 < toRow - fromR))
            return false;

        if (taking && (toColumn == fromColumn))
            return false;

        if (!taking && (toColumn != fromColumn))
            return false;

        if (taking && ((toRow - fromR == 2) || (toRow - fromR == -2)))
            return false;

        if (player == 0)
        {
            if (toRow < fromR)
                return false;

            if ((toRow - fromR == 2) && (toColumn != fromColumn))
                return false;

            if ((fromR != 1) && (toRow - fromR == 2))
                return false;

            if (taking && (toRow - fromR == 2))
                return false;

            if ((toRow - fromR == 2) && (battlefield[fromR + 1, fromColumn] != 0))
                return false;

            if (exchanging && toRow < 7)
                return false;

            if ((toRow == 7) && !exchanging)
                return false;
        }
        else
        {
            if (toRow > fromR)
                return false;

            if ((fromR - toRow == 2) && (toColumn != fromColumn))
                return false;

            if ((fromR != 6) && (fromR - toRow == 2))
                return false;

            if (taking && (fromR - toRow == 2))
                return false;

            if ((fromR - toRow == 2) && (battlefield[fromR - 1, fromColumn] != 0))
                return false;

            if (exchanging && 0 < toRow)
                return false;

            if ((toRow == 0) && !exchanging)
                return false;
        }

        return true;
    }

        #endregion

        #region Talking

    public static string MakeSentence()
    {
        sentence.Clear();
        if (playerToMove == 0)
            sentence.Append("White ");
        else
            sentence.Append("Black ");

        if (give == States.Resign)
            sentence.Append("- Resigns!");
        else if ((gotMove == drawQuestion)
                    && ((gameMode == GameModes.PlayerVsPlayer) || (gameMode == GameModes.PlayerVsComputer) || (gameMode == GameModes.ComputerVsComputer)))
            sentence.Append("wants Draw ?");
        else if (gotMove == drawByAgreement)
        {
            sentence.Clear();
            if (playerToMove == 0)
                sentence.Append("Black accepts draw by agreement!");
            else
                sentence.Append("White accepts draw by agreement!");
        }
        else
        {
            if (castle != Castles.noCastle)
            {
                if ((castle == Castles.castleKingsideFree) || (castle == Castles.castleKingsideCheck) || (castle == Castles.castleKingsideDraw) || (castle == Castles.castleKingsideCheckmate))
                    sentence.Append("makes castle kingside");
                else
                    sentence.Append("makes castle queenside");
            }
            else
            {
                switch (savePositions[playerToMove][0][0] % 10)
                {
                    case kingUnits: { sentence.Append("king on "); break; }
                    case queenUnits: { sentence.Append("queen on "); break; }
                    case rookUnits: { sentence.Append("rook on "); break; }
                    case bishopUnits: { sentence.Append("bishop on "); break; }
                    case nightUnits: { sentence.Append("night on "); break; }
                    case pawnUnits: { sentence.Append("pawn on "); break; }
                    default: break;
                }

                sentence.Append((char)(savePositions[playerToMove][0][2] + 97));
                sentence.Append(" " + (savePositions[playerToMove][0][1] + 1));
                if (takes)
                    sentence.Append(" takes");
                else
                    sentence.Append(" moves to");

                sentence.Append(" " + (char)(savePositions[playerToMove][1][2] + 97));
                sentence.Append(" " + (savePositions[playerToMove][1][1] + 1));
                if (exchange)
                {
                    sentence.Append(" and exchanges with ");
                    switch (exchangeWith % 10)
                    {
                        case kingUnits: { sentence.Append("king"); break; }
                        case queenUnits: { sentence.Append("queen"); break; }
                        case rookUnits: { sentence.Append("rook"); break; }
                        case bishopUnits: { sentence.Append("bishop"); break; }
                        case nightUnits: { sentence.Append("night"); break; }
                        case pawnUnits: { sentence.Append("pawn"); break; }
                        default: break;
                    }
                }
            }

            switch (give)
            {
                case States.Free: { sentence.Append("."); break; }
                case States.Check: { sentence.Append(" - Check!"); break; }
                case States.Draw: { sentence.Append(" - Draw!"); break; }
                case States.Checkmate: { sentence.Append(" - Checkmate!"); break; }
                default: break;
            }
        }

        return sentence.ToString();
    }

    public static void SayMove(string sayThis)
    {
        using (SpeechSynthesizer say = new SpeechSynthesizer())
        {
            say.SetOutputToDefaultAudioDevice();
            say.Rate = 0;
            //say.Volume = 100;
            say.Speak(sayThis);
        }
    }

        #endregion

    #endregion

    #region Make move

    public static void MakeMove(int fromFigure, int fromRow, int fromColumn, int toFigure, int toRow, int toColumn)
    {
        battlefield[fromRow, fromColumn] = fromFigure;
        battlefield[toRow, toColumn] = toFigure;
    }

    #endregion

    #region Check for check

    public static bool Checks(int player)
    {
        oppositeKing = figures[1 - player][0][0];
        kingRow = figures[player][1][0];
        kingColumn = figures[player][2][0];

        #region Queen and Rook

        for (theRow = kingRow - 1; (0 <= theRow) && (theRow <= 7); theRow--)
            if (battlefield[theRow, kingColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, kingColumn] == kingUnits - queenUnits)
                        || (oppositeKing - battlefield[theRow, kingColumn] == kingUnits - rookUnits))
                    return true;

                break;
            }

        for (theRow = kingRow + 1; (0 <= theRow) && (theRow <= 7); theRow++)
            if (battlefield[theRow, kingColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, kingColumn] == kingUnits - queenUnits)
                        || (oppositeKing - battlefield[theRow, kingColumn] == kingUnits - rookUnits))
                    return true;

                break;
            }

        for (theColumn = kingColumn - 1; (0 <= theColumn) && (theColumn <= 7); theColumn--)
            if (battlefield[kingRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[kingRow, theColumn] == kingUnits - queenUnits)
                        || (oppositeKing - battlefield[kingRow, theColumn] == kingUnits - rookUnits))
                    return true;

                break;
            }

        for (theColumn = kingColumn + 1; (0 <= theColumn) && (theColumn <= 7); theColumn++)
            if (battlefield[kingRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[kingRow, theColumn] == kingUnits - queenUnits)
                        || (oppositeKing - battlefield[kingRow, theColumn] == kingUnits - rookUnits))
                    return true;

                break;
            }

        #endregion

        #region Queen and Bishop

        theColumn = kingColumn;
        columnChange = -1;
        for (theRow = kingRow - 1; 0 <= theRow; theRow--)
        {
            theColumn += columnChange;
            if (theColumn < 0)
                break;

            if (battlefield[theRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, theColumn] == kingUnits - queenUnits)
                    || (oppositeKing - battlefield[theRow, theColumn] == kingUnits - bishopUnits))
                    return true;

                break;
            }
        }

        theColumn = kingColumn;
        columnChange = 1;
        for (theRow = kingRow - 1; 0 <= theRow; theRow--)
        {
            theColumn += columnChange;
            if (7 < theColumn)
                break;

            if (battlefield[theRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, theColumn] == kingUnits - queenUnits)
                    || (oppositeKing - battlefield[theRow, theColumn] == kingUnits - bishopUnits))
                    return true;

                break;
            }
        }

        theColumn = kingColumn;
        columnChange = -1;
        for (theRow = kingRow + 1; theRow <= 7; theRow++)
        {
            theColumn += columnChange;
            if (theColumn < 0)
                break;

            if (battlefield[theRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, theColumn] == kingUnits - queenUnits)
                    || (oppositeKing - battlefield[theRow, theColumn] == kingUnits - bishopUnits))
                    return true;

                break;
            }
        }

        theColumn = kingColumn;
        columnChange = 1;
        for (theRow = kingRow + 1; theRow <= 7; theRow++)
        {
            theColumn += columnChange;
            if (7 < theColumn)
                break;

            if (battlefield[theRow, theColumn] != 0)
            {
                if ((oppositeKing - battlefield[theRow, theColumn] == kingUnits - queenUnits)
                    || (oppositeKing - battlefield[theRow, theColumn] == kingUnits - bishopUnits))
                    return true;

                break;
            }
        }

        #endregion

        #region Night

        if ((0 <= kingRow - 2) && (0 <= kingColumn - 1) && (oppositeKing - battlefield[kingRow - 2, kingColumn - 1] == kingUnits - nightUnits))
            return true;

        if ((0 <= kingRow - 1) && (0 <= kingColumn - 2) && (oppositeKing - battlefield[kingRow - 1, kingColumn - 2] == kingUnits - nightUnits))
            return true;

        if ((kingRow + 1 <= 7) && (0 <= kingColumn - 2) && (oppositeKing - battlefield[kingRow + 1, kingColumn - 2] == kingUnits - nightUnits))
            return true;

        if ((kingRow + 2 <= 7) && (0 <= kingColumn - 1) && (oppositeKing - battlefield[kingRow + 2, kingColumn - 1] == kingUnits - nightUnits))
            return true;

        if ((kingRow + 2 <= 7) && (kingColumn + 1 <= 7) && (oppositeKing - battlefield[kingRow + 2, kingColumn + 1] == kingUnits - nightUnits))
            return true;

        if ((kingRow + 1 <= 7) && (kingColumn + 2 <= 7) && (oppositeKing - battlefield[kingRow + 1, kingColumn + 2] == kingUnits - nightUnits))
            return true;

        if ((0 <= kingRow - 1) && (kingColumn + 2 <= 7) && (oppositeKing - battlefield[kingRow - 1, kingColumn + 2] == kingUnits - nightUnits))
            return true;

        if ((0 <= kingRow - 2) && (kingColumn + 1 <= 7) && (oppositeKing - battlefield[kingRow - 2, kingColumn + 1] == kingUnits - nightUnits))
            return true;

        #endregion

        #region Pawn

        if (battlefield[kingRow, kingColumn] == whiteDecimals + kingUnits)
        {
            if ((kingRow + 1 <= 7) && (0 <= kingColumn - 1) && (oppositeKing - battlefield[kingRow + 1, kingColumn - 1] == kingUnits - pawnUnits))
                return true;

            if ((kingRow + 1 <= 7) && (kingColumn + 1 <= 7) && (oppositeKing - battlefield[kingRow + 1, kingColumn + 1] == kingUnits - pawnUnits))
                return true;
        }
        else
        {
            if ((0 <= kingRow - 1) && (0 <= kingColumn - 1) && (oppositeKing - battlefield[kingRow - 1, kingColumn - 1] == kingUnits - pawnUnits))
                return true;

            if ((0 <= kingRow - 1) && (kingColumn + 1 <= 7) && (oppositeKing - battlefield[kingRow - 1, kingColumn + 1] == kingUnits - pawnUnits))
                return true;
        }

        #endregion

        return false;
    }

    #endregion

    #region Opposite's Chance

    public static bool OppositeAvoid()
    {
        for (figureIndex = 0; figureIndex < figures[oppositePlayer][0].Count; figureIndex++)
        {
            ClearAllFieldsForLooking();
            if (IsThereLegalMoveThisFigure())
                return true;
        }

        return false;
    }

    public static bool IsThereLegalMoveThisFigure()
    {
        SaveOppositePositionsWithoutRealTakes(oppositePlayer, 0, figures[oppositePlayer][0][figureIndex], figures[oppositePlayer][1][figureIndex], figures[oppositePlayer][2][figureIndex]);

        king = figures[playerToMove][0][0];
        oppositeKing = figures[oppositePlayer][0][0];

        thisKing = oppositeKing;
        otherKing = king;

        figureRow = savePositions[oppositePlayer][0][1];
        figureColumn = savePositions[oppositePlayer][0][2];

        switch (figures[oppositePlayer][0][figureIndex] % 10)
        {
            case kingUnits: return LegalMovesThisKing(true);
            case queenUnits: return LegalMovesThisQueen(true);
            case rookUnits: return LegalMovesThisRook(true);
            case bishopUnits: return LegalMovesThisBishop(true);
            case nightUnits: return LegalMovesThisNight(true);
            case pawnUnits: return LegalMovesThisPawn(true);
            default: return false;
        }
    }

    #endregion

    #region Legal moves on this figure

    public static bool LegalMovesThisKing(bool stopAtFirstMeeting)
    {
        if (stopAtFirstMeeting)
        {
            KingMovement(playerToMove);
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveThisKing(oppositePlayer, toThisField))
                    return true;
        }
        else
        {
            KingMovement(oppositePlayer);
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveThisKing(playerToMove, toThisField))
                    AddPossibleMove();
        }

        return false;
    }

    public static bool LegalMovesThisQueen(bool stopAtFirstMeeting)
    {
        QeenMovement();
        if (stopAtFirstMeeting)
        {
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(oppositePlayer, toThisField))
                    return true;
        }
        else
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(playerToMove, toThisField))
                    AddPossibleMove();

        return false;
    }

    public static bool LegalMovesThisRook(bool stopAtFirstMeeting)
    {
        RookMovement();
        if (stopAtFirstMeeting)
        {
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(oppositePlayer, toThisField))
                    return true;
        }
        else
        {
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(playerToMove, toThisField))
                    AddPossibleMove();
        }

        return false;
    }

    public static bool LegalMovesThisBishop(bool stopAtFirstMeeting)
    {
        BishopMovement();
        if (stopAtFirstMeeting)
        {
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(oppositePlayer, toThisField))
                    return true;
        }
        else
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(playerToMove, toThisField))
                    AddPossibleMove();

        return false;
    }

    public static bool LegalMovesThisNight(bool stopAtFirstMeeting)
    {
        NightMovement();
        if (stopAtFirstMeeting)
        {
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(oppositePlayer, toThisField))
                    return true;
        }
        else
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(playerToMove, toThisField))
                    AddPossibleMove();

        return false;
    }

    public static bool LegalMovesThisPawn(bool stopAtFirstMeeting)
    {
        if (stopAtFirstMeeting)
        {
            PawnMovement(oppositePlayer);
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(oppositePlayer, toThisField))
                    return true;
        }
        else
        {
            PawnMovement(playerToMove);
            for (toThisField = 0; toThisField < lookOnlyThisPositions[0].Count; toThisField++)
                if (TryMakeMoveNotKing(playerToMove, toThisField))
                    AddPossibleMove();
        }

        return false;
    }

    public static bool TryMakeMoveThisKing(int player, int toThisField)
    {
        SaveOppositePositionsWithoutRealTakes(player, 1, lookOnlyThisPositions[0][toThisField], lookOnlyThisPositions[1][toThisField], lookOnlyThisPositions[2][toThisField]);
        figures[player][1][0] = savePositions[player][1][1];
        figures[player][2][0] = savePositions[player][1][2];
        MakeMove(0, savePositions[player][0][1], savePositions[player][0][2], savePositions[player][0][0], savePositions[player][1][1], savePositions[player][1][2]);
        willBeUnderCheck = Checks(player);
        figures[player][1][0] = savePositions[player][0][1];
        figures[player][2][0] = savePositions[player][0][2];
        MakeMove(savePositions[player][0][0], savePositions[player][0][1], savePositions[player][0][2], savePositions[player][1][0], savePositions[player][1][1], savePositions[player][1][2]);
        return !willBeUnderCheck;
    }

    public static bool TryMakeMoveNotKing(int player, int toThisField)
    {
        SaveOppositePositionsWithoutRealTakes(player, 1, lookOnlyThisPositions[0][toThisField], lookOnlyThisPositions[1][toThisField], lookOnlyThisPositions[2][toThisField]);
        MakeMove(0, savePositions[player][0][1], savePositions[player][0][2], savePositions[player][0][0], savePositions[player][1][1], savePositions[player][1][2]);
        willBeUnderCheck = Checks(player);
        MakeMove(savePositions[player][0][0], savePositions[player][0][1], savePositions[player][0][2], savePositions[player][1][0], savePositions[player][1][1], savePositions[player][1][2]);
        return !willBeUnderCheck;
    }

    public static void SaveOppositePositionsWithoutRealTakes(int player, int fromOrTo, int figure, int row, int column)
    {
        savePositions[player][fromOrTo][0] = figure;
        savePositions[player][fromOrTo][1] = row;
        savePositions[player][fromOrTo][2] = column;
    }

    #endregion

    #region Movements

    public static void KingMovement(int player)
    {
        kingRow = figures[player][1][0];
        kingColumn = figures[player][2][0];
        for (rowChange = -1; rowChange <= 1; rowChange++)
            for (columnChange = -1; columnChange <= 1; columnChange++)
                if (!((rowChange == 0) && (columnChange == 0)))
                {
                    rowMovement = figureRow + rowChange;
                    columnMovement = figureColumn + columnChange;
                    if ((0 <= rowMovement) && (rowMovement <= 7) && (0 <= columnMovement) && (columnMovement <= 7))
                    {
                        theField = battlefield[rowMovement, columnMovement];
                        if (((theField <= thisKing - kingUnits) || (thisKing < theField))
                                && (theField != otherKing)
                                    && ((rowMovement - kingRow < -1) || (1 < rowMovement - kingRow) || (columnMovement - kingColumn < -1) || (1 < columnMovement - kingColumn)))
                            AddFieldForLooking(theField, rowMovement, columnMovement);
                    }
                }
    }

    public static void QeenMovement()
    {
        RookMovement();
        BishopMovement();
    }

    public static void RookMovement()
    {
        rowMovement = figureRow;
        for (columnMovement = figureColumn - 1; 0 <= columnMovement; columnMovement--)
        {
            theField = battlefield[rowMovement, columnMovement];
            if (theField == 0)
                AddFieldForLooking(theField, rowMovement, columnMovement);
            else
            {
                if ((otherKing - kingUnits < theField) && (theField < otherKing))
                    AddFieldForLooking(theField, rowMovement, columnMovement);

                break;
            }
        }

        for (columnMovement = figureColumn + 1; columnMovement <= 7; columnMovement++)
        {
            theField = battlefield[rowMovement, columnMovement];
            if (theField == 0)
                AddFieldForLooking(theField, rowMovement, columnMovement);
            else
            {
                if ((otherKing - kingUnits < theField) && (theField < otherKing))
                    AddFieldForLooking(theField, rowMovement, columnMovement);

                break;
            }
        }

        columnMovement = figureColumn;
        for (rowMovement = figureRow - 1; 0 <= rowMovement; rowMovement--)
        {
            theField = battlefield[rowMovement, columnMovement];
            if (theField == 0)
                AddFieldForLooking(theField, rowMovement, columnMovement);
            else
            {
                if ((otherKing - kingUnits < theField) && (theField < otherKing))
                    AddFieldForLooking(theField, rowMovement, columnMovement);

                break;
            }
        }

        for (rowMovement = figureRow + 1; rowMovement <= 7; rowMovement++)
        {
            theField = battlefield[rowMovement, columnMovement];
            if (theField == 0)
                AddFieldForLooking(theField, rowMovement, columnMovement);
            else
            {
                if ((otherKing - kingUnits < theField) && (theField < otherKing))
                    AddFieldForLooking(theField, rowMovement, columnMovement);

                break;
            }
        }
    }

    public static void BishopMovement()
    {
        BishopMovementAdding(-1, -1);
        BishopMovementAdding(-1, 1);
        BishopMovementAdding(1, -1);
        BishopMovementAdding(1, 1);
    }

    public static void BishopMovementAdding(int rChange, int cChange)
    {
        columnMovement = figureColumn;
        for (rowMovement = figureRow + rChange; (0 <= rowMovement) && (rowMovement <= 7); rowMovement += rChange)
        {
            columnMovement += cChange;
            if ((0 <= columnMovement) && (columnMovement <= 7))
            {
                theField = battlefield[rowMovement, columnMovement];
                if (theField == 0)
                    AddFieldForLooking(theField, rowMovement, columnMovement);
                else
                {
                    if ((otherKing - kingUnits < theField) && (theField < otherKing))
                        AddFieldForLooking(theField, rowMovement, columnMovement);

                    break;
                }
            }
            else
                break;
        }
    }

    public static void NightMovement()
    {
        NightMovementAdding(figureRow - 2, figureColumn - 1);
        NightMovementAdding(figureRow - 2, figureColumn + 1);
        NightMovementAdding(figureRow - 1, figureColumn - 2);
        NightMovementAdding(figureRow - 1, figureColumn + 2);
        NightMovementAdding(figureRow + 1, figureColumn - 2);
        NightMovementAdding(figureRow + 1, figureColumn + 2);
        NightMovementAdding(figureRow + 2, figureColumn - 1);
        NightMovementAdding(figureRow + 2, figureColumn + 1);

        //for (rowChange = -2; rowChange <= 2; rowChange++)
        //{
        //    rowMovement = figureRow + rowChange;
        //    if ((0 <= rowMovement) && (rowMovement <= 7))
        //        for (columnChange = -2; columnChange <= 2; columnChange++)
        //        {
        //            columnMovement = figureColumn + columnChange;
        //            theField = battlefield[rowMovement, columnMovement];
        //            if ((0 <= columnMovement) && (columnMovement <= 7)
        //                && (rowChange * rowChange + columnChange * columnChange == 5)
        //                    && ((theField <= oppositeKing - kingUnits) || (oppositeKing < theField))
        //                        && (theField != king))
        //                AddFieldForLooking(theField, rowMovement, columnMovement);
        //        }
        //}
    }

    public static void NightMovementAdding(int rMovement, int cMovement)
    {
        if ((0 <= rMovement) && (rMovement <= 7) && (0 <= cMovement) && (cMovement <= 7))
        {
            theField = battlefield[rMovement, cMovement];
            if (((theField <= thisKing - kingUnits) || (thisKing < theField))
                    && (theField != otherKing))
                AddFieldForLooking(theField, rMovement, cMovement);
        }
    }

    public static void PawnMovement(int player)
    {
        if (player == 0)
            rowChange = 1;
        else
            rowChange = -1;

        rowMovement = figureRow + rowChange;
        if ((0 <= rowMovement) && (rowMovement <= 7))
        {
            for (columnChange = -1; columnChange <= 1; columnChange++)
            {
                columnMovement = figureColumn + columnChange;
                if ((0 <= columnMovement) && (columnMovement <= 7))
                {
                    theField = battlefield[rowMovement, columnMovement];
                    if (columnMovement != figureColumn)
                    {
                        if ((otherKing - kingUnits < theField) && (theField < otherKing))
                            AddFieldForLooking(theField, rowMovement, columnMovement);
                    }
                    else
                    {
                        if (theField == 0)
                        {
                            AddFieldForLooking(theField, rowMovement, columnMovement);
                            if (((player == 0) && (figureRow == 1)) || ((player == 1) && (figureRow == 6)))
                            {
                                rowMovement += rowChange;
                                theField = battlefield[rowMovement, columnMovement];
                                if (theField == 0)
                                    AddFieldForLooking(theField, rowMovement, columnMovement);

                                rowMovement -= rowChange;
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Looking array

    public static void ClearAllFieldsForLooking()
    {
        lookOnlyThisPositions[0].Clear();
        lookOnlyThisPositions[1].Clear();
        lookOnlyThisPositions[2].Clear();
    }

    public static void AddFieldForLooking(int figure, int row, int column)
    {
        lookOnlyThisPositions[0].Add(figure);
        lookOnlyThisPositions[1].Add(row);
        lookOnlyThisPositions[2].Add(column);
    }

    #endregion

    #region Artificial Intelligence

    public static void FindAllPossibleMoves()
    {
        ClearAllPossibleMoves();
        for (figureIndex = 0; figureIndex < figures[playerToMove][0].Count; figureIndex++)
        {
            ClearAllFieldsForLooking();

            possibleMoves.Add(new List<List<int>>());
            possibleMoves[figureIndex].Add(new List<int>() { figureIndex, figures[playerToMove][0][figureIndex], figures[playerToMove][1][figureIndex], figures[playerToMove][2][figureIndex] });

            SaveOppositePositionsWithoutRealTakes(playerToMove, 0, figures[playerToMove][0][figureIndex], figures[playerToMove][1][figureIndex], figures[playerToMove][2][figureIndex]);
            savePositions[playerToMove][0][3] = figureIndex;

            king = figures[playerToMove][0][0];
            oppositeKing = figures[oppositePlayer][0][0];

            figureRow = savePositions[playerToMove][0][1];
            figureColumn = savePositions[playerToMove][0][2];

            thisKing = king;
            otherKing = oppositeKing;

            switch (figures[playerToMove][0][figureIndex] % 10)
            {
                case kingUnits: LegalMovesThisKing(false); break;
                case queenUnits: LegalMovesThisQueen(false); break;
                case rookUnits: LegalMovesThisRook(false); break;
                case bishopUnits: LegalMovesThisBishop(false); break;
                case nightUnits: LegalMovesThisNight(false); break;
                case pawnUnits: LegalMovesThisPawn(false); break;
                default: break;
            }
        }

        for (i = possibleMoves.Count - 1; 0 <= i; i--)
            if (possibleMoves[i].Count < 2)
                possibleMoves.RemoveAt(i);
    }

    public static void ClearAllPossibleMoves()
    {
        possibleMoves.Clear();
    }

    public static void AddPossibleMove()
    {
        possibleMoves[figureIndex].Add(new List<int>() { figureIndex, savePositions[playerToMove][1][0], savePositions[playerToMove][1][1], savePositions[playerToMove][1][2] });
    }

    public static void ChoosePossibleMove()
    {
        takes = false;
        exchange = false;

        randomIndex = randomGenerator.Next(0, possibleMoves.Count);
        randomOnePossibleMovement = randomGenerator.Next(1, possibleMoves[randomIndex].Count);
        if (possibleMoves[randomIndex][randomOnePossibleMovement][1] != 0)
        {
            takes = true;
            savePositions[playerToMove][1][3] = ReturnIndexOfFigure(oppositePlayer, possibleMoves[randomIndex][randomOnePossibleMovement][2], possibleMoves[randomIndex][randomOnePossibleMovement][3]);
            materials[oppositePlayer] -= figures[oppositePlayer][0][savePositions[playerToMove][1][3]];
            takenFigures[oppositePlayer].Add(figures[oppositePlayer][0][savePositions[playerToMove][1][3]]);
            RemoveFigureAtPosition(oppositePlayer, savePositions[playerToMove][1][3]);
        }

        MakeMove(0, possibleMoves[randomIndex][0][2], possibleMoves[randomIndex][0][3], possibleMoves[randomIndex][0][1], possibleMoves[randomIndex][randomOnePossibleMovement][2], possibleMoves[randomIndex][randomOnePossibleMovement][3]);
        figures[playerToMove][1][possibleMoves[randomIndex][0][0]] = possibleMoves[randomIndex][randomOnePossibleMovement][2];
        figures[playerToMove][2][possibleMoves[randomIndex][0][0]] = possibleMoves[randomIndex][randomOnePossibleMovement][3];
        if ((figures[playerToMove][0][possibleMoves[randomIndex][0][0]] == figures[playerToMove][0][0] - kingUnits + pawnUnits)
            && (((playerToMove == 0) && (figures[playerToMove][1][possibleMoves[randomIndex][0][0]] == 7))
                || ((playerToMove == 1) && (figures[playerToMove][1][possibleMoves[randomIndex][0][0]] == 0))))
        {
            exchange = true;
            exchangeWith = randomGenerator.Next(figures[playerToMove][0][0] - kingUnits + pawnUnits + 1, figures[playerToMove][0][0]);
            battlefield[possibleMoves[randomIndex][randomOnePossibleMovement][2], possibleMoves[randomIndex][randomOnePossibleMovement][3]] = exchangeWith;
            figures[playerToMove][0][possibleMoves[randomIndex][0][0]] = exchangeWith;
            materials[playerToMove] += exchangeWith - (figures[playerToMove][0][0] - kingUnits) - pawnUnits;
        }

        if (Checks(oppositePlayer))
        {
            if (OppositeAvoid())
                give = States.Check;
            else
                give = States.Checkmate;
        }
        else
        {
            if (OppositeAvoid())
                give = States.Free;
            else
                give = States.Draw;
        }

        BuildTextOfMove();
    }

    public static void BuildTextOfMove()
    {
        buildTextOfMove.Clear();
        buildTextOfMove.Append(FigureSign(possibleMoves[randomIndex][0][1]));
        buildTextOfMove.Append((char)(possibleMoves[randomIndex][0][3] + 97));
        buildTextOfMove.Append((possibleMoves[randomIndex][0][2] + 1));
        if (takes)
        {
            randomTakesSymbol = randomGenerator.Next(0, 2);
            buildTextOfMove.Append(string.Format(randomTakesSymbol == 0 ? "x" : ":"));
        }
        else
            buildTextOfMove.Append("-");

        buildTextOfMove.Append((char)(possibleMoves[randomIndex][randomOnePossibleMovement][3] + 97));
        buildTextOfMove.Append((possibleMoves[randomIndex][randomOnePossibleMovement][2] + 1));
        if (exchange)
            buildTextOfMove.Append("=" + FigureSign(exchangeWith));

        if (give == States.Check)
            buildTextOfMove.Append("+");
        else if (give == States.Checkmate)
            buildTextOfMove.Append("#");
        else if (give == States.Draw)
            buildTextOfMove.Append("=");

        savePositions[playerToMove][0][0] = possibleMoves[randomIndex][0][1];
        savePositions[playerToMove][0][1] = possibleMoves[randomIndex][0][2];
        savePositions[playerToMove][0][2] = possibleMoves[randomIndex][0][3];
        savePositions[playerToMove][1][1] = possibleMoves[randomIndex][randomOnePossibleMovement][2];
        savePositions[playerToMove][1][2] = possibleMoves[randomIndex][randomOnePossibleMovement][3];
    }

    public static string FigureSign(int figure)
    {
        switch (figure % 10)
        {
            case nightUnits: return "N";
            case bishopUnits: return "B";
            case rookUnits: return "R";
            case queenUnits: return "Q";
            case kingUnits: return "K";
            default: return "";
        }
    }

    #endregion

    #region Players' figures

    //public static void MoveFigure(int player, int figure, int row, int column)
    //{
    //    for (i = 0; i < figures[player][0].Count; i++)
    //        if ((figures[player][0][i] == figure) && (figures[player][1][i] == row) && (figures[player][2][i] == column))
    //        {
    //            figures[player][1][i] = row;
    //            figures[player][2][i] = column;
    //            break;
    //        }
    //}

    //public static void RemoveFigure(int player, int figure, int row, int column)
    //{
    //    for (i = 0; i < figures[player][0].Count; i++)
    //        if ((figures[player][0][i] == figure) && (figures[player][1][i] == row) && (figures[player][2][i] == column))
    //        {
    //            RemoveFigureAtPosition(player, i);
    //            break;
    //        }
    //}

    public static void RemoveFigureAtPosition(int player, int index)
    {
        figures[player][0].RemoveAt(index);
        figures[player][1].RemoveAt(index);
        figures[player][2].RemoveAt(index);
    }

    //public static void AddFigure(int player, int figure, int row, int column)
    //{
    //    firstOfExchangeKind = figures[player][0].Count;
    //    for (i = 0; i < figures[player][0].Count; i++)
    //        if (figures[player][0][i] <= figure)
    //        {
    //            firstOfExchangeKind = i;
    //            break;
    //        }

    //    AddFigureAtPosition(player, firstOfExchangeKind, figure, row, column);
    //}

    public static void AddFigureAtPosition(int player, int index, int figure, int row, int column)
    {
        figures[player][0].Insert(index, figure);
        figures[player][1].Insert(index, row);
        figures[player][2].Insert(index, column);
    }

    #endregion

    #region Change player to move

    public static void ChangePlayerToMove()
    {
        oppositePlayer = playerToMove;
        playerToMove = 1 - playerToMove;
    }

    #endregion
}