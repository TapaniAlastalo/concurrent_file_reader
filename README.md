# Concurrent FileReader

TTOW0420 Concurrent Programming
Tapani Alastalo


## Final Project Description (Proposal)

To make concurrent file reader program. Program starts multiple threads which will read a file from different segments concurrently and sum the results at the end.

### Possible application for this solution

To read huge files containing strcutured data concurrently and combine results for data-analyze.


## Project Report

### Solution

Chosen solution used tasks to read data from file and process the acquired data concurrently. 

- The file (file length) reading was divided equally between chosen number of tasks.
- The data was read in bytes and encrypted to text (UTF-8) for processing. 
- Text were split by rows to text array
- First and last index of the text array were stored to string list. When adding to string list the list were locked by the task in turn.
- Other parts of the text array were parsed and number values were added to sum
- At last the string list were processed and parsed number values were added to sum. In this point there were no solutions added for integrity problems.

### Problems

The chosen solution brings integrity problems for the block of data that were read by tasks at first and last. Because the file reading is divided equally between the tasks it is very likely that tasks start or end reading in middle of data block. The severity of these problems depends on how the data is structured and handled. In this case the solution was set aside because this problem should be handled case by case.

#### Possible solutions

- Reject the blocks that are not entact in cases where a lot of data is processed and tiny part of data won't have any statistically important impact.
- Combine the blocks using string list (or dictionary etc) used in chosen solution
- Move the starting point where to read to get entact blocks by structuring data and by finding starting postions for the block(s).

### Efficiency tests

Purpose of the tests was to test how much (or not at all) does chosen concurrent method reduce file processing time. As a comparison sequential (one thread) way for processing the data on the file were also tested.

#### Test files

For test purposes, 5 sample files was built which included numbers from 1-20 per row.
Biggest files were from 37 - 110 MB which contained millions rows of data.
Smallest files were included for testing basic functionality and checksums.

#### Test cases

- Purpose was to read data from a file and sum up the numbers found from file. By summing up the numbers we also got checksum number. 
- Data reading and processing was done for different size of files (from 1 KB - 110 MB)
- Data reading and processing was done by sequential way to get comparison for concurrent method
- Data reading and processing was done by chosen concurrent method which used tasks for reading. Different amount of tasks were used from 1 - 256.

#### Results

Test results showed that data reading and processing time were reduced more than 30 % in best concurrent cases when compared to sequential way. Best concurrent results were achieved with 64 / 128 concurrent tasks with big files (37 - 110 MB).



## Schedule

| Topics                                | Time spent       |
|:--------------------------------------|:------------------|
| Planning and reading about subject | 2 h |
| Proposal and example          | 1 h |
| Project Coding and testing | 7 h |
| Report | 1 h |


## Sources

 * [ReaderWriterLock](https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlock?view=netframework-4.8)

