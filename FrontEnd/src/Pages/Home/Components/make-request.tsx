import { IconButton, Typography } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { Grid } from "@mui/material";
import React, { useState } from "react";
import MakeRequestModal from "../../Dashboard/Components/make-request-modal";
import Modal from "@mui/material/Modal";

const MakeARequest = () => {
  const [modalOpen, setModalOpen] = useState(false);
  const handleOpenModal = () => setModalOpen(true);
  const handleCloseModal = () => setModalOpen(false);
  const test = 4;

  return (
    <>
      <Grid container justifyContent="center" alignItems="center">
        <Grid item>
          <Typography variant="h6">
            Didn't find what you were looking for?
          </Typography>
        </Grid>
      </Grid>

      <Grid
        container
        justifyContent="center"
        alignItems="center"
        onClick={() => {
          handleOpenModal();
        }}
      >
        <Grid item>
          <IconButton color="primary" size="large">
            <AddCircleIcon fontSize="large" />
          </IconButton>
        </Grid>

        <Grid item>
          <Typography variant="h5">Make A Request</Typography>
        </Grid>
      </Grid>

      <Modal
        open={modalOpen}
        onClose={handleCloseModal}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <MakeRequestModal />
      </Modal>
    </>
  );
};
export default MakeARequest;
