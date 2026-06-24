context("Test api/members", () => {

    var memberId = 0;
    it("create a new member", () => {
        cy.request("POST", "/api/members", {
            "firstName": "John",
            "lastName": "Doe",
            "email": "jdoe@test.fr"
        }).then((response) => {
            expect(response.status).to.eq(201)
            expect(response.body).to.be.greaterThan(0)
            memberId = response.body;
        })
    })

    it("get the list of members", () => {
        cy.request("GET", "/api/members").then((response) => {
            expect(response.status).to.eq(200)
            expect(response.body).length.to.be.greaterThan(0)
        })
    })

    it("get the new member", () => {
        cy.request("GET", "/api/members/" + memberId).then((response) => {
            expect(response.status).to.eq(200)
            expect(response.body).to.not.be.empty
            expect(response.body).to.have.property('firstName', 'John')
            expect(response.body).to.have.property('lastName', 'Doe')
        })
    })

    it("update the new member", () => {
        cy.request("PUT", "/api/members/" + memberId, {
            "firstName": "Johnny",
            "lastName": "Begood",
            "email": "jdoe@test.fr"
        }).then((response) => {
            expect(response.status).to.eq(204)
        })
    })

    it("check the new member after update", () => {
        cy.request("GET", "/api/members/" + memberId).then((response) => {
            expect(response.status).to.eq(200)
            expect(response.body).to.not.be.empty
            expect(response.body).to.have.property('firstName', 'Johnny')
            expect(response.body).to.have.property('lastName', 'Begood')
        })
    })

    it("delete the new member", () => {
        cy.request("DELETE", "/api/members/" + memberId).then((response) => {
            expect(response.status).to.eq(204)
        })
    })

    it("check the new member was deleted", () => {
        cy.request({
            method: "GET",
            url: "/api/members/" + memberId,
            failOnStatusCode: false,
        }).then((response) => {
            expect(response.status).to.eq(404)
        })
    })
})
