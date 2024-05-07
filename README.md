# Overview

PipeWatch is a small utility to monitor DevOps pipeline runs and provide visibility into when
they finish. To get started, build and run the application, and provide the required settings
on first run.

The first time you run the applet you may see a lot of builds in the list if you have been building
actively in the past week. You can click the Dismiss link to hide these builds.

# Settings

## Build requestor of interest
This should be the email address you use with GE's DevOps instance, usually first.last@ge.com.
The applet will by default only show you runs that were triggered by that user.

If you want to monitor another run, navigate to that run in the web interface and drag the URL onto
the PipeWatch utility. The URL should look something like this (the `buildId` query parameter is
what PipeWatch needs):
 https://dev.azure.com/geaviationdigital-dss/EMS/_build/results?buildId=512984&view=results

## Your DevOps PAT
This should be a PAT that has Read access for Build and for Code. It does not need any Write access
and should not be provisioned with such.

## DevOps projects
This is a list of projects (not repositories!) whose pipelines you are interested in. All pipelines
within these project(s) will be checked for runs automatically.

