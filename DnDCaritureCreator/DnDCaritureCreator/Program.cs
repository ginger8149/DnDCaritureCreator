// See https://aka.ms/new-console-template for more information
using DnDCaritureCreator.models;
using DnDCaritureCreator.services;

Console.WriteLine("Hello, World!");

CharacterStats testStats = new CharacterStats();

testStats.constitution = 4;
testStats.strength = 1;


StatGenerator StatGenerator = new StatGenerator();

testStats = StatGenerator.PointBuy(testStats);

