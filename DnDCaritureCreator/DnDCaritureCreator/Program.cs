// See https://aka.ms/new-console-template for more information
using DnDCaritureCreator.models;
using DnDCaritureCreator.services;

Console.WriteLine("Hello, World!");

CharacterStats testStats = new CharacterStats();

StatGenerator StatGenerator = new StatGenerator();

testStats = StatGenerator.RolledStats(testStats);

