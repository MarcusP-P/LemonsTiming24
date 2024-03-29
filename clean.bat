@echo off

echo Removing .vs
rd /s /q .vs

echo Removing packages
rd /s /q packages

echo Cleaning Artifacts
rd /s /q artifacts

echo Deleting old artifacts. These are no longer created, but may be lying around still.
echo Cleaning Client
rd /s /q Client\bin
rd /s /q Client\obj

echo Cleaning Server
rd /s /q Server\bin
rd /s /q Server\obj

echo Cleaning Shared
rd /s /q Shared\bin
rd /s /q Shared\obj