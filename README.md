# Elemental Arena

## Overview
Elemental Arena is a fast paced top-down 2D shooter where players must use 3 elements (grass, fire, water) to eliminate all enemies off the board.

## Core Gameplay
- Players must move their controllable character around to dodge incoming attacks
- Swap between the three elements
- Use specific elements to defeat enemies of different types
- To win players must survive 3 total rounds, each of which are timed
- Players lose when their HP is depleted completely or do not finish the round in time

## Short Gameplay Video
https://youtu.be/jWxbvj577ts

## Game Type
Survival, Bullet-hell shooter

## Player Setup
- Single player as the main controllable character
- Optional local co-op where swapping elements also swaps players

## AI Design
### Basic enemy
- Move along fixed paths
- Fires basic projectiles at set intervals
- Player's element type is irrelevant

### Elemental enemy
- Fires off a specific type of attack (grass, fire, or water), can interchange type
- Changes behaviour (agression, speed, projectile type) based on elemental weakness or strength

## Scripted Events
- Once the player defeats all enemies on the board, a new level will begin. Total of 3 levels
- There is a boss at the end of the third level

## Physics Scope
- 2D physics using Rigidbody2D and Collider2D
- Projectiles use kinematic motion and trigger colliders to detect hits
- Level geometry includes obstacles with collision detection for cover mechanics
- Layer-based collision (Player, Enemy, Projectile, Obstacle) configured in physics settings

## FSM Scope
- Finite State Machines implemented for enemies and boss
- Basic FSM: Patrol → Chase → Attack → Reset
- Elemental FSM: Idle → Detect Element → Select Attack → Cooldown
- Transitions handled via Unity Events

## Environment
- Pixel-art style
- Each level has a elemental theme (volcano, coral reef, and forest)
- Levels have different shapes, potential for cover

## Systems and Mechanics
- Each element is weak against one element, and strong against another. 
(Nature is weak against fire, and strong against water
Water is weak against nature, and strong against fire
Fire is weak against water, and strong against nature)
- Levels begin with the player, enemies, and level obstacles in preset positions 
- Some enemies are "basic", and move on preset paths
- Some enemies are "elemental", and behave relative to the player's elemental type
- The player must fire projectiles at enemies at destroy them to progress to the next level
- Enemies fire projectiles at the player, which deal damage if they hit the player
- The amount damage dealt to players or enemies is based on the element. For example, while the player is fire type, they will deal extra damage to nature enemies and less damage to water enemies

## Controls
- Space to swap elements
- WASD to move
- G to shoot

- 2nd player: Up arrow, down arrow, left arrow, right arrow
- Enter to swap elements
- L to shoot

## Project Setup aligned to course topics
- Built in Unity (2021.3+ or latest stable)
- C# Scripts for PlayerController, EnemyAI, Projectile, ElementManager, LevelManager, BossController
- Uses 2D physics and collision layers for interaction
- Organized hierarchy (Player, Enemies, Projectiles, UI, Environment)
- GitHub Classroom repository with clear commits, branches, and tags# Elemental Arena
