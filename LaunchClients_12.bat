@echo off
start /wait /b Client.exe 127.0.0.1 -1920,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 -1440,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 -960,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 -1920,540 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 -1440,540 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 -960,540 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 1920,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 2400,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 2880,0 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 1920,540 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 2400,540 480,540
PING 1.1.1.1 -n 1 -w 500 >NUL
start /wait /b Client.exe 127.0.0.1 2880,540 480,540
