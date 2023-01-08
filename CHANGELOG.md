# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.1]
### Changed
- Upgrade dependency `Microsoft.Extensions.Options` to version `7.0.0`
- Upgrade Test and Example project to `net7`

## [2.0.0]
### Changed
- Change namespace from Scissors.HttpRequestInterceptor to Scissors
- Rename RequestsInterceptor to HttpRequestInterceptor
- Rename HttpInterceptorOptions to HttpRequestInterceptorOptions

## [1.0.1]
### Changed
- Switch from Nuget package source to GitHub
- Rename project to Scissors.HttpRequestInterceptor.*

## [1.0.0]
### Added
- Added action for pushing nuget package
### Changed
- Renamed project to Scissors.HttpRequestInterceptor.*


## [0.3.1]
### Changed
- Light refactoring

## [0.3.0]
### Added
- Added more test
### Changed
- Upgrade Tests and Example to .net 
- Fix bug where expected 502 would return 502
- Fix bug with returning null on not configured return status, and now exception is thrown

## [0.2.0] - 2020-06-28
### Changed
- Small refactoring and cleanup.
- Upgrade Tests and Example to .net core 3.1

## [0.1.0] - 2019-07-07
### Added
- First version of RequestInterceptor.