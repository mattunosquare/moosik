import { TextField } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Button } from "@mui/material";
import { Grid } from "@mui/material";
import { useState } from "react";
import { makeARecommendation } from "../Services/ThreadService";
import { IMakeARecommendationForm } from "../Common/Interfaces";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface IProps {
  closeModal: () => void;
}

const MakeRecommendationModal = ({ closeModal }: IProps) => {
  const [title, setTitle] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [postResourceTitle, setPostResourceTitle] = useState<string>("");

  return (
    <div>
      <Box sx={style}>
        <Typography id="modal-header" variant="h6" component="h2">
          Make A Recommendation
        </Typography>

        <Typography id="modal-subtitle" sx={{ mt: 2 }}>
          What would you like to recommend this music for?
        </Typography>

        <TextField
          id="title-field"
          label="Title"
          variant="filled"
          fullWidth
          value={title}
          onChange={(e) => {
            setTitle(e.target.value);
          }}
        />

        <Typography id="description-subtitle" sx={{ mt: 2 }}>
          Any further details to provide?
        </Typography>

        <TextField
          id="description-field"
          label="Further Details"
          fullWidth
          multiline
          variant="filled"
          rows={4}
          onChange={(e) => {
            setDescription(e.target.value);
          }}
        />

        <Typography id="post-resources-subtitle" sx={{ mt: 2 }}>
          Please provide link.
        </Typography>

        <TextField
          id="post-resources-field"
          label="Please provide a link."
          fullWidth
          multiline
          variant="filled"
          rows={4}
          onChange={(e) => {
            setPostResourceTitle(e.target.value);
          }}
        />
        <Grid container justifyContent="flex-end" gap="3%" sx={{ mt: "7%" }}>
          <Button
            size="small"
            onClick={() => {
              closeModal();
            }}
          >
            Cancel
          </Button>
          <Button
            size="small"
            onClick={() => {
              const formData = {
                Title: title,
                PostDescription: description,
                PostResourceTitle: postResourceTitle,
              };
              makeARecommendation(formData);
              closeModal();
            }}
          >
            Save
          </Button>
        </Grid>
      </Box>
    </div>
  );
};

export default MakeRecommendationModal;
