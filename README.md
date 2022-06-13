I used the .net core 5 approach to make it work and build in vs2019. 
I ended up downgrading the entire project to .net core 3.1 when I implemented the FileContextCore. I then realized I could just downgrade the EF Core and re-upgraded the project to .net core 5. But before this I actually implemented 
some of the missing methods in FileContextCore to make it work with the latest EF Core. I then rolled back to keep it standard according to the assessment.

I have made use of the repository pattern to keep it database agnostic in a way as well as following some of the SOLID principles. 

I would have liked to add more than one frontend to demonstrate my skill across the board but I ran out of time :)

