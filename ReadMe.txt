HLU Tool Database Updater 1.0.1

Copyright © 2014 Sussex Biodiversity Record Centre

Overview
--------
The HLU Tool Database Updater provides an automated mechanism of applying
structural and/or content changes to a target HLU Tool relational database.
It will process one or more script files and execute all the SQL commands
in the files.

The program does not need to be installed - it can be run as a standalone
program and will connect to any of the HLU Tool relation database formats,
including Access, SQL Server, PostgreSQL and Oracle (although connections to
PostgreSQL and Oracle have not been tested).

Requirements
------------
The HLU Tool Database Updater requires the following:
 - Microsoft Windows XP/2003/2008/Vista/Windows 7 or Windows 8 
 - 3 GHz or higher processor
 - 2 GB RAM
 - 3 GB of free hard disk space
      (For increased performance a multiple core processor with as much RAM
		as possible is recommended)

 - Microsoft .NET Framework 3.5 SP1, 4.0, or 4.5 installed. 
      (You can download .NET Framework 3.5 and its Service Pack here)

Instructions
------------
The program does not need to be installed and can be run as a standalone program
by any user with administrator privileges. To run the program:
- Ensure that any scripts to be processed are placed in a 'Scripts' folder
  directly under the program path.
- Double-click the program to start it.
- Click 'Connect' to connect the to target database to be updated.
- Follow the instructions to select the connection type and target database.
- Click 'Ok' once the connection has been established.
- Click 'Proceed' to process the script files found in the 'Scripts' folder.
- Progress bars and a text window will show the progress and success of the
  processing.
- Click 'Close' once all scripts have been processed.
- If you have more than one target HLU relational database copy the script
  files from the 'Archive' folder in the 'Scripts' folder back into the
  'Scripts' folder and re-run the program again - connecting to the next
  database to be updated.

Source Code
-----------
The source code for the HLU GIS Tool is open source and can be downloaded from:
<https://github.com/HabitatFramework/HLUTool-DatabaseUpdater>

Documentation
-------------
The user guide for the latest release (and for earlier releases) will be available
online or downloadable as a PDF in the near future from:
<https://readthedocs.org/projects/hlutool-technicalguide/>

Issue Reporting
---------------
To search for existing known issues please use:
<https://github.com/HabitatFramework/HLUTool-DatabaseUpdater/issues>

To report new issues please use:
<http://forum.lrcs.org.uk/viewforum.php?id=24>

License Information
-------------------
The HLU Tool Database Updater is free software. You can redistribute it and/or
modify it under the terms of the GNU General Public License as published by the
Free Software Foundation, either version 3 of the License, or (at your option)
any later version.

See the file "License.txt" supplied with the program for information on the
terms & conditions for usage and copying, and a DISCLAIMER OF ALL WARRANTIES
or see http://www.gnu.org/licenses for more details of the GNU General Public
License.

--------------------------------------------------------------------------------
