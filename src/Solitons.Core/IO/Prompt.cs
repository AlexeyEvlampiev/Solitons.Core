using System;

namespace Solitons.IO;

/// <summary>
/// A utility class that prompts for user input in console applications.
/// </summary>
public static class Prompt
{
    /// <summary>
    /// Prompts the user with the specified message and expects a 'yes' or 'no' response.
    /// The user can enter 'y', 'n', 'yes', or 'no' (case insensitive).
    /// </summary>
    /// <param name="promptMessage">The message to display to the user.</param>
    /// <returns>True if the user responds with 'yes' or 'y', false if the user responds with 'no' or 'n'.</returns>
    /// <exception cref="System.FormatException">Thrown when user input is neither 'yes' nor 'no'.</exception>
    public static bool GetYesNoAnswer(string promptMessage)
    {
        while (true)
        {
            Console.Write($"{promptMessage} (y/n): ");

            // Read the user input
            string? response = Console.ReadLine()?.Trim().ToLower();

            // Check if the response is yes or no
            if (response == "y" || response == "yes")
                return true;
            else if (response == "n" || response == "no")
                return false;

            // If the input is not valid, show an error message and try again
            Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
        }
    }
}