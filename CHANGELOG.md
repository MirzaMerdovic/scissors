# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2019-07-07
### Changes
- First version of RequestInterceptor.

## [0.2.0] - 2020-06-28
### Changes
- Small refactoring and cleanup.
- Upgraded Tests and Example to .net core 3.1

## [0.3.0]
### Changes
- Upgraded Tests and Example to .net 5
- Added more test
- Fixed bug where expected 502 would return 502
- Fixed bug with returning null on not configured return status, and now exception is thrown