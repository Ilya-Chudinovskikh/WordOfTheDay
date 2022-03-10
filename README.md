# WordOfTheDay
1. Install the app using Scripts\WordOfTheDay.Install.ps1.
2. Run the app via Scripts\WordOfTheDay.Run.ps1.
3. To run application locally without Docker use Scripts\WordOfTheDay.Run.Dotnet.ps1.

Some extra information about the app:
Business requirements for the main part of application:
1)	user with the same email cannot write more than one word a day
2)	word must be written in english without spaces and special characters, max 50 symbols
3)	when the user clicks on “Tell the whole world!” he sees:
    •	word of the day (starting from 00:00 UTC up till now)
    •	number of people who wrote the word of the day
    •	his word if it doesn’t match the word of the day
    •	number of people who wrote his word
    •	statistics of closest words (with one letter difference)
      ex: User’s word: life, words shown in statistics: lite, lif, live, wife etc.
