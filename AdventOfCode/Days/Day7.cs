using AdventOfCode.Common;

namespace AdventOfCode.Days;

#pragma warning disable CS8602

public class Day7 : DayTask
{
    public Day7(string path) : base(path) 
    {
        Initialize();
    }

    SortedSet<Hand> Hands = new(Comparer<Hand>.Default);
    SortedSet<Hand> JokeredHands = new(Comparer<Hand>.Default);
    static readonly CardCountComparer cardCountComparer = new CardCountComparer();

    public enum HandKind
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public enum Card
    {
        Joker, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
        Jack, Queen, King, Ace
    }

    Card InterpretCard(char c, bool withJoker = false)
    {
        return c switch
        {
            '2' => Card.Two,
            '3' => Card.Three,
            '4' => Card.Four,
            '5' => Card.Five,
            '6' => Card.Six,
            '7' => Card.Seven,
            '8' => Card.Eight,
            '9' => Card.Nine,
            'T' => Card.Ten,
            'J' => withJoker ? Card.Joker : Card.Jack,
            'Q' => Card.Queen,
            'K' => Card.King,
            'A' => Card.Ace,
            _ => throw new ArgumentException("Unknown Card")
        };
    }

    public class CardCountComparer : Comparer<KeyValuePair<Card, int>>
    {
        public override int Compare(KeyValuePair<Card, int> x, KeyValuePair<Card, int> y)
        {
            return -x.Value.CompareTo(y.Value);
        }
    }

    public class Hand : IComparable<Hand>
    {
        public HandKind Kind { get; set; }
        public int Bet { get; set; }
        public List<Card>? Cards { get; set; }

        public int CompareTo(Hand? other)
        {
            if (Kind - other.Kind != 0) return Kind - other.Kind;
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i] - other.Cards[i] != 0) return Cards[i] - other.Cards[i];
            }
            return 0;
        }

        public static HandKind GetHandKind(IEnumerable<Card> cards, bool withJokers = false)
        {
            var values = new Dictionary<Card, int>();
            foreach (var card in cards)
            {
                if (!values.TryAdd(card, 1))
                {
                    values[card]++;
                }
            }

            if (withJokers)
            {
                var jokersCount = values.GetValueOrDefault(Card.Joker);
                if (jokersCount > 0)
                {
                    values.Remove(Card.Joker);
                    var tmp = values.ToList();
                    tmp.Sort(cardCountComparer);
                    if(values.Count > 0)
                    {
                        values[tmp[0].Key] += jokersCount;
                    }
                    else // 5 jokers case
                    {
                        values.Add(Card.Joker, jokersCount);
                    }
                }
            }

            var counts = values.ToList();
            counts.Sort(cardCountComparer);

            return counts.Count switch
            {
                1 => HandKind.FiveOfAKind,
                2 => counts[0].Value == 4 ? HandKind.FourOfAKind : HandKind.FullHouse,
                3 => counts[0].Value == 3 ? HandKind.ThreeOfAKind : HandKind.TwoPair,
                4 => HandKind.OnePair,
                _ => HandKind.HighCard
            };
        }
    }

    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    Hand hand = new();
                    Hand jokeredHand = new();

                    var tmp = line.Split(' ');
                    hand.Bet = int.Parse(tmp[1]);
                    jokeredHand.Bet = int.Parse(tmp[1]);

                    hand.Cards = new List<Card> 
                    { 
                        InterpretCard(tmp[0][0]), 
                        InterpretCard(tmp[0][1]), 
                        InterpretCard(tmp[0][2]), 
                        InterpretCard(tmp[0][3]), 
                        InterpretCard(tmp[0][4])
                    };

                    jokeredHand.Cards = new List<Card>
                    {
                        InterpretCard(tmp[0][0], withJoker: true),
                        InterpretCard(tmp[0][1], withJoker: true),
                        InterpretCard(tmp[0][2], withJoker: true),
                        InterpretCard(tmp[0][3], withJoker: true),
                        InterpretCard(tmp[0][4], withJoker: true)
                    };

                    hand.Kind = Hand.GetHandKind(hand.Cards);
                    jokeredHand.Kind = Hand.GetHandKind(jokeredHand.Cards, withJokers: true);

                    Hands.Add(hand);
                    JokeredHands.Add(jokeredHand);
                }
            }
        }
    }

    public override void PartOne()
    {
        var tmpHands = Hands.ToList();
        long sum = 0;
        for (int i = 0 ; i < tmpHands.Count; i++)
        {
            sum += tmpHands[i].Bet * (i + 1);
        }
        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        var tmpHands = JokeredHands.ToList();
        long sum = 0;
        for (int i = 0; i < tmpHands.Count; i++)
        {
            sum += tmpHands[i].Bet * (i + 1);
        }
        Console.WriteLine(sum);
    }
}
