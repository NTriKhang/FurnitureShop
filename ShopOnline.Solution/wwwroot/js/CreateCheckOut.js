function CreateCheckOut(sessionId, pubKey) {
    console.log(sessionId);
    const stripe = Stripe(pubKey);
    console.log(stripe);
    stripe.redirectToCheckout({ sessionId });
}