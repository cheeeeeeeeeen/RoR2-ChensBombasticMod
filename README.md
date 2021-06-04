![Version](https://img.shields.io/badge/Version-2.0.3-orange)
![Build](https://github.com/cheeeeeeeeeen/RoR2-ChensBombasticMod/workflows/Build/badge.svg)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Support Chen](https://img.shields.io/badge/Support-Chen-ff69b4)](https://ko-fi.com/cheeeeeeeeeen)

[![GitHub issues](https://img.shields.io/github/issues/cheeeeeeeeeen/RoR2-ChensBombasticMod)](https://github.com/cheeeeeeeeeen/RoR2-ChensBombasticMod/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/cheeeeeeeeeen/RoR2-ChensBombasticMod)](https://github.com/cheeeeeeeeeen/RoR2-ChensBombasticMod/pulls)
![Maintenance Status](https://img.shields.io/badge/Maintainance-Active-brightgreen)

# Chen's Bombastic Mod

## Description

Adds 2 Artifacts into the game which are related to the Artifact of Spite.

## Installation

Use **[Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager)** to install this mod.

If one does not want to use a mod manager, then get the DLL from **[Thunderstore](https://thunderstore.io/package/Chen/ChensBombasticMod/)**.

## New Artifacts
- **Malice** : Enemies generate bombs *upon spawning*.
- **Spleen** : Enemies *have a chance* to generate bombs *upon being attacked*.

## Contact
- Issue Page: https://github.com/cheeeeeeeeeen/RoR2-ChensBombasticMod/issues
- Email: `blancfaye7@gmail.com`
- Discord: `Chen#1218`
- RoR2 Modding Server: https://discord.com/invite/5MbXZvd
- Give a tip through Ko-fi: https://ko-fi.com/cheeeeeeeeeen

## Changelog

**2.0.3**
- Fix another bug where Spleen manager limit does not follow 0 being limitless.

**2.0.2**
- Fix a silly bug.

**2.0.1**
- Add a config option for to have a limit of the maximum bombs there can be in the queue.
- The config option does not equate to limiting the total number of bombs in the playing field, since the issue being addressed here is the queue generating more than it can process.

**2.0.0**
- Update the mod so that it works again after the anniversary update.

**1.0.9**
- Integrate QueueProcessors from ChensHelpers.

**1.0.8**
- Code cleanup and update the dependency as there were improvements from ChensHelpers.

**1.0.7**
- Update the mod to improve some implementations from ChensHelper.
- Needed update as they were breaking changes.

**1.0.6**
- Update dependency to breaking changes with ChensHelpers.
- Update access modifiers of classes.

**1.0.5**
- Integrate ChensHelpers.
- There shouldn't be any notable changes in game play.

**1.0.4**
- Remove debug build!

**1.0.3**
- Fix an issue on connected clients lagging hard.
- Rollback version dependency of R2API to `2.5.14` as it was causing some weird issues.

**1.0.2**
- Quick bug fix regarding the Bombastic Manager component being added twice when both artifacts are enabled.
- Change default percent chance value to 15% since 5% looks like it doesn't proc. Configurable.

**1.0.1**
- Rename the mod in manifest to the correct one. Whoops.

**1.0.0**
- Add the artifacts into the game.