# DeadlockApp
Simple app to produce deadlocks and blocking processes for alerts in SQL Monitor.

Command line only, works with the following arguments:

instance database  - Creates 2 tables and 5 sprocs on your database, and produces a deadlock.
instance database block - Creates 2 tables and 5 sprocs on your database, and produces some blocking processes which last about 10 minutes.

No argument parsing, no error handling, just does the job.
