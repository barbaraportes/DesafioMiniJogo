/*
 RECURSOS DO MINIJOGO:

    Um recurso para determinar se o jogador consumiu os alimentos
    Um recurso que atualiza o status do jogador dependendo dos alimentos consumidos
    Um recurso que pausa a velocidade de movimento dependendo dos alimentos consumidos
    Um recurso para regenerar o alimento em um novo local
    Uma opção para encerrar o jogo se um caractere sem suporte for pressionado
    Um recurso para encerrar o jogo se a janela do Terminal foi redimensionada

*/


using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Posição do player
int playerX = 0;
int playerY = 0;

// Posição da comida
int foodX = 0;
int foodY = 0;

// Player e comidas disponiveis
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// String do jogador atual exibida no console
string player = states[0];

// Index da comida atual
int food = 0;

InitializeGame();
while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console foi redimensionado.");
        Console.WriteLine("Saindo do programa...");
        shouldExit = true;
    }
    else
    {
        if (PlayerIsFaster())
        {
            Move(1, false);
        }
        else if (PlayerIsSick())
        {
            FreezePlayer();
        }
        else
        {
            Move(otherKeysExit: false);
        }
        if (GotFood())
        {
            ChangePlayer();
            ShowFood();
        }
    }
}

// Retorna true se o terminal for redimensionado
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Exibe comida aleatoria em local aleatorio
void ShowFood()
{
    // Atualiza comida para aleatorio index
    food = random.Next(0, foods.Length);

    // Atualiza posição da comida para local aleatorio
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Exibe a comida no local
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Retorna true se a localizãção do player corresponde com localização da comida
bool GotFood()
{
    return playerY == foodY && playerX == foodX;
}

// Retorna true se o player está com aparência doente
bool PlayerIsSick()
{
    return player.Equals(states[2]);
}

// Retorna true se o player está com aparência rápido 
bool PlayerIsFaster()
{
    return player.Equals(states[1]);
}

// Muda o player para corresponder com a comida consumida
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Impede temporariamente o player de se mover
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Lê a entrada direcional do console e move o player
void Move(int speed = 1, bool otherKeysExit = false)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX -= speed;
            break;
        case ConsoleKey.RightArrow:
            playerX += speed;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            // Exit se outra tecla for pressionada
            shouldExit = otherKeysExit;
            break;
    }

    // Limpa os caracteres da posição anterior
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Mantem a posição do player dentro dos limites da janela do terminal
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Desenha o player em nova localização
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Limpa o console, exibe a comida e o player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}