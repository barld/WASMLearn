function getPrimesBelow(n) {
    const primes = [2, 3]
    for (let i = 5; i < n; i+= 2) 
    {
        const sqrtI = Math.sqrt(i);
        let isPrime = true;
        for (let j = 3; j < sqrtI; j += 2)
        {
            if (i % j == 0) {
                isPrime = false;
                break;
            }
        }

        if (isPrime) {
            primes.push(i);
        }
    }

    return primes;
}

window.performTest = function (n) {
    let t0 = performance.now();
    let numbersCount = getPrimesBelow(n).length;
    let t1 = performance.now();

    return `Amount: ${numbersCount}. within: ${t1 - t0} ms.`;
}