# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
Dates are in ISO-8601 standart (trying to be)

## [Unreleased]

## [1.0.4] - 2022-07-22
### Added
- protected constructor to ComponentObjectPooler
### Changed
- ObjectPooler initialPoolCount now properly initilizes into stack

## [1.0.3] - 2022-07-22
### Added
- This CHANGELOG.md file
- LICENSE.md file
- All proper .meta files to added files above
### Changed
- Fixed install instructions url in README.md
- Added refference to this CHANGELOG.md from README.md

## [1.0.2] - 2022-07-20
### Added
- Transform container property in ObjectPooler for situations when we need to return objects to pool, but only have reference to pool

## [1.0.2] - 2022-07-20
### Changed
- Fix CheckForCreatedObjects() in ComponentObjectPooler for propper Destroy logic
- Fix ObjectPooler constructor to properly check initialPoolCount after CheckForCreatedObjects()